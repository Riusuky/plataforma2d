using UnityEngine;

public enum GyroscopeServiceStatus {
	DISABLED,
	STOPPED,
	RUNNING,
	FAILED
}

public class GyroscopeCompassController : MonoBehaviour
{
	[HideInInspector]
	public GyroscopeServiceStatus status = GyroscopeServiceStatus.DISABLED;
	public Transform targetObject;
	public bool useCompassCorrection = false;
	public bool useOwnTransform = false;
	public bool runOnStart = true;
	[HideInInspector]
	public bool updateTransform = false;
	public bool tryToUseGyroscope = true;
	public bool tryToUseAccelerometer = true;
	public float timeOutPeriod = 4f;
	
	private Transform cameraReference;
	private Quaternion cameraReferenceNewRotation;
	private Quaternion lastCameraReferenceRotation;
	private float slerpPeriod = 2f;
	private float timeSinceLastRotationUpdate = 0;
	private bool compassDisabledOnStart;
	private bool firstOrientationSet = false;
	private bool neverMoved = true;
	private float timePassed = 0;
	
	private void Start() {
		if(useOwnTransform) {
			targetObject = transform;
		}
		
		if(runOnStart) {
			updateTransform = true;
		}
		
		if(tryToUseGyroscope) {
			Input.gyro.enabled = true;
		}
		else if (!tryToUseAccelerometer) {
			Debug.Log("Sensor was not selected.");
			status = GyroscopeServiceStatus.FAILED;
		}
		else if(!useCompassCorrection) {
			compassDisabledOnStart = true;
			useCompassCorrection = true;
		}
		
		if(useCompassCorrection) {
			InitializeCompassService();
		}
	}
	
	private void Update() {
		//Debug.Log("Status-------------------> "+status.ToString());
		switch(status) {
			case GyroscopeServiceStatus.DISABLED:
			{
				if(updateTransform) {
					if(useCompassCorrection){
						if( (GPSServiceController.status != GPSServiceStatus.DISABLED) && (GPSServiceController.status != GPSServiceStatus.FAILED) && (GPSServiceController.status != GPSServiceStatus.INITIALIZING) ) {							
							status = GyroscopeServiceStatus.RUNNING;
						}
						else if(GPSServiceController.status == GPSServiceStatus.FAILED) {
							status = GyroscopeServiceStatus.FAILED;
						}
					}
					else {
						status = GyroscopeServiceStatus.RUNNING;
					}
				}
			}
				break;
			case GyroscopeServiceStatus.FAILED:
			{
			}
				break;
			case GyroscopeServiceStatus.RUNNING:
			{
				if(GPSServiceController.status == GPSServiceStatus.FAILED) {
					Debug.Log("GPS signal lost.");
					status = GyroscopeServiceStatus.FAILED;
				}
				if(updateTransform) {
					if( tryToUseGyroscope && Input.gyro.enabled ) {
						UpdateTargetTransformWithGyroscope();
					}
					else if( tryToUseAccelerometer ) {
						UpdateTargetTransformWithAccelerometer();
					}
					else {
						status = GyroscopeServiceStatus.FAILED;
					}
				}
				else {
					status = GyroscopeServiceStatus.STOPPED;
				}
			}
				break;
			case GyroscopeServiceStatus.STOPPED:
			{
				if(updateTransform) {
					status = GyroscopeServiceStatus.RUNNING;
				}
			}
				break;
		}
	}
	
	private void InitializeCompassService() {
		Input.compass.enabled = true;
		if(GPSServiceController.status == GPSServiceStatus.DISABLED) {
			StartCoroutine(GPSServiceController.InitializeServiceLocation());
		}
	}
	
	private void UpdateTargetTransformWithGyroscope() {
		if(targetObject != null) {
			if(useCompassCorrection) {
				if( (Input.gyro.attitude.eulerAngles.x == 0) && (Input.gyro.attitude.eulerAngles.y == 0) && (Input.gyro.attitude.eulerAngles.z == 0) && neverMoved ) {
					timePassed += Time.deltaTime;
					
					if(timePassed > timeOutPeriod) {
						Debug.Log("Gyroscope attitude is null.");
						
						tryToUseGyroscope = false;
						Input.gyro.enabled = false;
						
						timePassed = 0;
						
						return;
					}
				}
				else {
					neverMoved = false;
				}
				
				if(!firstOrientationSet) {
					cameraReference = (new GameObject("CameraReference")).transform;
					cameraReference.position = targetObject.position;
					cameraReference.rotation = Quaternion.Euler(90f, 0, 0);
					targetObject.SetParent(cameraReference);
					cameraReferenceNewRotation = cameraReference.rotation;
					lastCameraReferenceRotation = cameraReference.rotation;
					firstOrientationSet = true;
				}
				
				timeSinceLastRotationUpdate += Time.deltaTime;
				
				Vector3 zDirection = Input.compass.rawVector;
				zDirection.z *= -1f;
				
				Vector3 yDirection = Input.gyro.gravity;
				yDirection.z *= -1f;
				yDirection = -yDirection.normalized;
				
				zDirection = zDirection - Vector3.Dot(zDirection, yDirection)*yDirection;
				zDirection = zDirection.normalized;
				
				Vector3 fDirection = new Vector3(0, 0, 1f);
				fDirection = fDirection - Vector3.Dot(fDirection, yDirection)*yDirection;
				fDirection = fDirection.normalized;
				
				float compassAngle = Mathf.Rad2Deg*Mathf.Sign(Vector3.Dot(Vector3.Cross(zDirection, fDirection), yDirection))*Mathf.Acos(Vector3.Dot(zDirection, fDirection));
				compassAngle = compassAngle < 0 ? compassAngle + 360f : compassAngle;
				if( Mathf.Abs(targetObject.rotation.eulerAngles.y + (cameraReferenceNewRotation.eulerAngles.y - cameraReference.rotation.eulerAngles.y) - compassAngle) > 15f) {
					float actualAngularDifference = targetObject.rotation.eulerAngles.y - cameraReferenceNewRotation.eulerAngles.y;
					actualAngularDifference = actualAngularDifference < 0 ? actualAngularDifference + 360f : actualAngularDifference;
					cameraReferenceNewRotation = Quaternion.Euler (90f, compassAngle - actualAngularDifference, 0);
					lastCameraReferenceRotation = cameraReference.rotation;
					timeSinceLastRotationUpdate = 0;
				}
				
				targetObject.localRotation = Quaternion.Euler(-Input.gyro.attitude.eulerAngles.x, -Input.gyro.attitude.eulerAngles.y, Input.gyro.attitude.eulerAngles.z);
				cameraReference.rotation = Quaternion.Slerp(lastCameraReferenceRotation, cameraReferenceNewRotation, timeSinceLastRotationUpdate/slerpPeriod);
			}
			else {
				if( (Input.gyro.rotationRateUnbiased.x == 0) && (Input.gyro.rotationRateUnbiased.y == 0) && (Input.gyro.rotationRateUnbiased.z == 0) && neverMoved ) {
					timePassed += Time.deltaTime;
					
					if(timePassed > timeOutPeriod) {
						Debug.Log("Gyroscope value is null.");
						
						tryToUseGyroscope = false;
						Input.gyro.enabled = false;
						
						if(tryToUseAccelerometer) {
							useCompassCorrection = true;
							InitializeCompassService();
							status = GyroscopeServiceStatus.DISABLED;
						}
						
						timePassed = 0;
						
						return;
					}
				}
				else {
					neverMoved = false;
				}
				
				if (!firstOrientationSet) {
					Vector3 yDirection = (Input.gyro.gravity.x == 0) && (Input.gyro.gravity.y == 0) && (Input.gyro.gravity.z == 0) ? -Input.acceleration : -Input.gyro.gravity.normalized;
					yDirection.z *= -1f;
					
					Vector3 randomDirection = new Vector3 (Random.Range (-10f, 10f), Random.Range (-10f, 10f), Random.Range (-10f, 10f));
					Vector3 zDirection = Vector3.Cross (randomDirection, yDirection);
					
					while (zDirection.magnitude < 0.0001f) {
						//Debug.Log("randomDirection: "+randomDirection+" yDirection: "+yDirection+" zDirection: "+zDirection+" -- " +zDirection.magnitude);
						randomDirection = new Vector3 (Random.Range (-10f, 10f), Random.Range (-10f, 10f), Random.Range (-10f, 10f));
						zDirection = Vector3.Cross (randomDirection, yDirection);
					}
					
					zDirection = zDirection.normalized;
					
					Vector3 xDirection = Vector3.Cross (yDirection, zDirection).normalized;
					
					Matrix4x4 transformationMatrix = Matrix4x4.identity;
					transformationMatrix.SetRow (0, new Vector4 (xDirection.x, yDirection.x, zDirection.x));
					transformationMatrix.SetRow (1, new Vector4 (xDirection.y, yDirection.y, zDirection.y));
					transformationMatrix.SetRow (2, new Vector4 (xDirection.z, yDirection.z, zDirection.z));
					
					targetObject.LookAt (targetObject.position + transformationMatrix.inverse.MultiplyPoint3x4 (new Vector3 (0, 0, 1f)), transformationMatrix.inverse.MultiplyPoint3x4 (new Vector3 (0, 1f, 0)));
					//Debug.Log(xDirection + " " + yDirection + " " + zDirection);
					firstOrientationSet = true;
				} 
				else {
					Vector3 deltaRotation = -Input.gyro.rotationRateUnbiased;
					deltaRotation.z *= -1f;
					targetObject.Rotate(deltaRotation, Space.Self);
				}
			}
		}
	}
	
	private void UpdateTargetTransformWithAccelerometer() {
		if((Input.compass.rawVector.x == 0) && (Input.compass.rawVector.y == 0) && (Input.compass.rawVector.z == 0) && neverMoved) {
			timePassed += Time.deltaTime;
			
			if(timePassed > timeOutPeriod) {
				if(compassDisabledOnStart) {
					Debug.Log("Compass value is null (Compass is required to use the accelerometer to ajust orientation).");
				}
				else {
					Debug.Log("Compass value is null.");
				}
				
				tryToUseAccelerometer = false;
				
				return;
			}
		}
		else {
			neverMoved = false;
		}
		
		Vector3 yDirection = Input.acceleration.normalized * -1f;
		yDirection.z *= -1f;
		Vector3 zDirection = Input.compass.rawVector;
		zDirection.z *= -1f;
		Vector3 xDirection = new Vector3((yDirection.y * zDirection.z) - (yDirection.z * zDirection.y), (yDirection.z * zDirection.x) - (yDirection.x * zDirection.z), (yDirection.x * zDirection.y) - (yDirection.y * zDirection.x));
		xDirection = xDirection.normalized;
		
		zDirection -= Vector3.Dot(zDirection, yDirection) * yDirection + Vector3.Dot(zDirection, xDirection) * xDirection;
		zDirection = zDirection.normalized;
		
		Matrix4x4 transformationMatrix = Matrix4x4.identity;
		transformationMatrix.SetRow(0, new Vector4(xDirection.x, yDirection.x, zDirection.x));
		transformationMatrix.SetRow(1, new Vector4(xDirection.y, yDirection.y, zDirection.y));
		transformationMatrix.SetRow(2, new Vector4(xDirection.z, yDirection.z, zDirection.z));
		
		targetObject.LookAt(targetObject.position + transformationMatrix.inverse.MultiplyPoint3x4(new Vector3(0, 0, 1f)), transformationMatrix.inverse.MultiplyPoint3x4(new Vector3(0, 1f, 0)));
		
		//Debug.Log ( "gravity: "+yDirection+" compass: "+zDirection+ " Inverse: "+ " final-> "+transformationMatrix.inverse.MultiplyPoint(new Vector3(0, 0, 1f))+" up-> "+transformationMatrix.inverse.MultiplyPoint3x4(new Vector3(0, 1f, 0)) );
	}
}

