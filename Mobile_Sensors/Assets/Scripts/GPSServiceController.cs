using UnityEngine;
using System.Collections;

public enum GPSServiceStatus {
	DISABLED,
	INITIALIZING,
	STOPPED,
	RUNNING,
	FAILED
}

public struct RelativePosition {
	public float distance;
	public float distanceError;

	public float relativeAngle;
	public float relativeAngleError;

	public RelativePosition(float _distance, float _relativeAngle, float _distanceError = 0, float _relativeAngleError = 0) {
		distance = _distance;
		distanceError = _distanceError;
		relativeAngle = _relativeAngle;
		relativeAngleError = _relativeAngleError;
	}
}

public class GPSServiceController : MonoBehaviour
{
	public static GPSServiceStatus status = GPSServiceStatus.DISABLED;
	public static float minimumAccuracy = 100f;
	public static float minimumDistanceUpdate = 8f;
	public static bool serviceLocationTimeOutReached = false;
	public static LocationInfo lastLocation{
		get{
			if(status == GPSServiceStatus.RUNNING){
				_lastLocation = Input.location.lastData;
				return _lastLocation;
			}
			else {
				return _lastLocation;
			}
		}
	}

	private static LocationInfo _lastLocation;
	private static bool hasInstance = false;
	private static GPSServiceController instance;

	public float refreshPeriod = 0;
	public bool startRefreshUpdate = false;

	private float timeSinceLastUpdate = 0;

	public static IEnumerator InitializeServiceLocation() {
		if(status == GPSServiceStatus.DISABLED) {
			if(!Input.location.isEnabledByUser) {
				status = GPSServiceStatus.FAILED;
				yield break;
			}

			status = GPSServiceStatus.INITIALIZING;
			
			Input.location.Start(minimumAccuracy, minimumDistanceUpdate);
			
			int maxWait = 20;
			while ( (Input.location.status == LocationServiceStatus.Initializing) && (maxWait > 0) )
			{
				yield return new WaitForSeconds(1);
				maxWait--;
			}
			
			if (maxWait < 1)
			{
				Debug.Log("GPS Timeout. Could not initialize service location.");
				status = GPSServiceStatus.FAILED;
				yield break;
			}
			
			if (Input.location.status == LocationServiceStatus.Failed)
			{
				Debug.Log("Failed to initialize service location.");
				status = GPSServiceStatus.FAILED;
				yield break;
			}
			else
			{
				status = GPSServiceStatus.RUNNING;
				_lastLocation = Input.location.lastData;
			}
		}
	}

	public static RelativePosition CalculateRelativePosition(Vector2 targetCoordinate) {
		Vector2 currentPosition = new Vector2(lastLocation.latitude, lastLocation.longitude);

		RelativePosition relativePosition = new RelativePosition();

		relativePosition.distance = CalculateDistance2(currentPosition, targetCoordinate);
		relativePosition.distanceError = lastLocation.horizontalAccuracy;
		relativePosition.relativeAngle = CalculateRelativeAngle(currentPosition, targetCoordinate);
		relativePosition.relativeAngleError = CalculateAngularError(relativePosition.distance, relativePosition.distanceError);

		return relativePosition;
	}

	//haverSine
	public static float CalculateDistance(Vector2 pointA, Vector2 pointB) {
		float earthRadius = 6371000f;
		float deltaLatitude = (pointB.x - pointA.x)*Mathf.Deg2Rad;
		float deltaLongitude = (pointB.y - pointA.y)*Mathf.Deg2Rad;
		
		//Debug.Log ("Deltas: "+deltaLatitude+" "+deltaLongitude);
		
		float a = Mathf.Sin (deltaLatitude)*Mathf.Sin (deltaLatitude) + Mathf.Cos (deltaLatitude)*Mathf.Cos (deltaLongitude)*Mathf.Sin (deltaLongitude)*Mathf.Sin (deltaLongitude);
		//Debug.Log ("a: "+a);
		float c = 2f*Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1-a));
		//Debug.Log ("c: "+c);
		float d = earthRadius * c;
		
		return d;
	}
	
	//ellipsoidal
	public static float CalculateDistance2(Vector2 pointA, Vector2 pointB) {
		float meanLatitude = Mathf.Deg2Rad*(pointA.x + pointB.x) / 2f;
		float k1 = 111.13209f - 0.56605f*Mathf.Cos(2f*meanLatitude) + 0.0012f*Mathf.Cos(4f*meanLatitude);
		float k2 = 111.41513f * Mathf.Cos (meanLatitude) - 0.09455f * Mathf.Cos (3f * meanLatitude) + 0.00012f * Mathf.Cos (5f * meanLatitude);
		float deltaLatitude = (pointB.x - pointA.x);
		float deltaLongitude = (pointB.y - pointA.y);
		
		float d = Mathf.Sqrt(Mathf.Pow(k1*deltaLatitude, 2f) + Mathf.Pow(k2*deltaLongitude, 2f))*1000f;
		
		return d;
	}

	public static float CalculateRelativeAngle(Vector2 pointA, Vector2 pointB) {
		float deltaLongitude = (pointB.y - pointA.y)*Mathf.Deg2Rad;
		float y = Mathf.Sin(deltaLongitude) * Mathf.Cos(pointB.x * Mathf.Deg2Rad);
		float x = Mathf.Cos(pointA.x * Mathf.Deg2Rad) * Mathf.Sin(pointB.x * Mathf.Deg2Rad) - Mathf.Sin(pointA.x * Mathf.Deg2Rad) * Mathf.Cos(pointB.x * Mathf.Deg2Rad) * Mathf.Cos(deltaLongitude);

		float bearing = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

		Debug.Log(bearing);

		return bearing;
	}

	public static float CalculateAngularError(float distance, float positionError) {
		float angularError = Mathf.Atan2(distance, positionError) * Mathf.Rad2Deg;

		Debug.Log(angularError);

		return angularError;
	}

	public static void SetRefreshPeriod(float _refreshPeriod) {
		if(hasInstance) {
			if(instance != null) {
				instance.refreshPeriod = _refreshPeriod;
				instance.startRefreshUpdate = true;
			}
			else {
				Debug.Log("Instance is null when it should not.");
			}
		}
		else {
			instance = (new GameObject("GPSRefreshUpdater")).AddComponent<GPSServiceController>();
			instance.refreshPeriod = _refreshPeriod;
			instance.startRefreshUpdate = true;
		}
	}

	//Still requires testing.
	public static void UpdateLastPosition() {
		if(status == GPSServiceStatus.RUNNING) {
			Input.location.Stop();
			Input.location.Start(minimumAccuracy, minimumDistanceUpdate);
		}
		else if(status == GPSServiceStatus.STOPPED) {
			Input.location.Start(minimumAccuracy, minimumDistanceUpdate);
			Input.location.Stop();
		}
		else {
			Debug.Log("GPS service is not running.");
		}
	}

	public static bool CheckForInstanceExistance() {
		return instance != null ? true : false;
	}

	private void Start() {
		if(hasInstance) {
			Destroy(gameObject);
		}
		else {
			hasInstance = true;
			instance = this;
			StartCoroutine(InitializeServiceLocation());
		}
	}

	private void Update() {
		if(status == GPSServiceStatus.FAILED) {
			Debug.Log("GPS service could not be initialized. Refresh update on GPS is being canceled.");
			Destroy(gameObject);
		}

		if(startRefreshUpdate) {
			timeSinceLastUpdate += Time.deltaTime;
			if (timeSinceLastUpdate > refreshPeriod) {
				if (Input.location.status == LocationServiceStatus.Stopped) {
					Input.location.Start(minimumAccuracy, minimumDistanceUpdate);
					status = GPSServiceStatus.RUNNING;
				}
				if (Input.location.status == LocationServiceStatus.Running) {
					_lastLocation = Input.location.lastData;
					Input.location.Stop();
					status = GPSServiceStatus.STOPPED;
					timeSinceLastUpdate = 0.0f;
				}
			}
		}
	}
}

