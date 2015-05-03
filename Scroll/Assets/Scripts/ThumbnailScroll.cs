using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum ScaleMethod{
	NONE,
	LINEAR,
	QUADRATIC,
	COSINUSOIDAL
}

public enum SeparatorMethod{
	NONE,
	LINEAR,
	SINUSOIDAL
}

public struct ThumbnailStruct{
	public string name;
	public Sprite sprite;
	public string URL;
}

public class ThumbnailScroll : MonoBehaviour
{
	public RectTransform thumbnailPrefab;
	public ScrollRect thumbnailScrollRect;
	public RectTransform thumbnailPanel;
	public RectTransform thumbnailObjectGroup;
	[Header("Scale")]
	public float thumbnailMaxScale = 1.3f;
	public float relativeStepScale = 2f;
	public ScaleMethod scaleMethod = ScaleMethod.COSINUSOIDAL;
	[Header("Separator")]
	public bool useSmoothSwitch = false;
	public float separatorMultiplier = 1f;
	public float relativeStepSeparator = 0.5f;
	public SeparatorMethod separatorMethod = SeparatorMethod.SINUSOIDAL;
	[Header("Distribution")]
	public float relativeStepDistance = 1.2f;
	[Header("Behavior")]
	public bool activateHighlight = false;
	public float focusPeriod = 0.6f;
	public bool useInertia = false;
	public float minimumVelocity = 200f;
	[Header("Test area")]
	public bool createThumbnailsOnStart = false;
	public int numberToCreate = 10;
	public Sprite testSprite;

	private bool thumbnailIsSet = false;
	private int numberOfThumbnails = 0;
	private Vector2 thumbnailSize;
	private float thumbnailDeltaPosition = 0;
	private float[] thumbnailScrollPosition;
	private Vector2[] thumbnailPositions;
	private RectTransform[] thumbnailList;
	private int currentThumbnail = 0;
	private int lastCurrentThumbnail = 0;
	private bool onFocusThumbnail = false;
	private bool triggerThumbnailEventAtEndOfFocus = false;
	private bool isDragging = false;
	private int thumbnailIndexOnBegginDrag = 0;
	private bool checkInertia = false;
	//private float onFocusTimer = 0;

	private void Start() {
		//Testing! 
		if(createThumbnailsOnStart) {
			ThumbnailStruct[] covers = new ThumbnailStruct[numberToCreate];
			for(int i = 0; i<covers.Length; i++) {
				covers[i].name = i.ToString();
				covers[i].URL = i.ToString();
				covers[i].sprite = testSprite;
			}

			ReadPages(covers);
		}

		if(useInertia) {
			if(!thumbnailScrollRect.inertia) {
				thumbnailScrollRect.inertia = true;
			}
		}
		else {
			if(thumbnailScrollRect.inertia) {
				thumbnailScrollRect.inertia = false;
			}
		}
	}

	private void Update() {
		if(checkInertia) {
			if(Mathf.Abs(thumbnailScrollRect.velocity.x) <= minimumVelocity) {
				thumbnailScrollRect.velocity.Set(0, 0);
				isDragging = false;
				checkInertia = false;
				FocusCurrentThumbnail();
			}
		}

		/* Without iTween, it would be something like this.
		if(onFocusThumbnail) {
			onFocusTimer += Time.deltaTime;
			if(onFocusTimer/focusPeriod < 1f) {
				OnFocusCurrentThumbnail(Mathf.Lerp(thumbnailScrollRect.horizontalNormalizedPosition, thumbnailScrollPosition[currentThumbnail], onFocusTimer/focusPeriod));
			}
			else {
				OnFocusCurrentThumbnail(thumbnailScrollPosition[currentThumbnail]);
				onFocusTimer = 0;
				OnEndFocusCurrentThumbnail();
			}
		}
		 */
	}
	public void ReadPages(ThumbnailStruct[] thumbnails) {
		bool panelGroupWasDeactive = ActivatePanelGroup();
		
		thumbnailIsSet = false;
		
		numberOfThumbnails = thumbnails.Length;

		thumbnailSize = new Vector2(thumbnailPrefab.rect.width, thumbnailPrefab.rect.height);
		
		if(numberOfThumbnails > 1) {
			thumbnailDeltaPosition = 1.0f / (numberOfThumbnails - 1.0f);
		}
		else {
			thumbnailDeltaPosition = 50.0f;
		}

		if(useSmoothSwitch) {
			float thumbnailHalfStepSize = new float();
			float halfSeparationOffset = new float();

			switch(scaleMethod) {
				case ScaleMethod.NONE:
					{
						thumbnailHalfStepSize = 1f;
					}
					break;
				case ScaleMethod.LINEAR:
					{
						thumbnailHalfStepSize = LinearScaleFunction(thumbnailDeltaPosition / 2.0f);
					}
					break;
				case ScaleMethod.QUADRATIC:
					{
						thumbnailHalfStepSize = QuadraticScaleFunction(thumbnailDeltaPosition / 2.0f);
					}
					break;
				case ScaleMethod.COSINUSOIDAL:
				{
					thumbnailHalfStepSize = CosinusoidalScaleFunction(thumbnailDeltaPosition / 2.0f);
				}
					break;
			}

			halfSeparationOffset = ((thumbnailSize.x * (thumbnailHalfStepSize - 1.0f) + (1f - relativeStepDistance) * thumbnailSize.x))/2f;

			switch(separatorMethod) {
				case SeparatorMethod.NONE:
				{
					relativeStepSeparator = 0.5f;
					separatorMultiplier = halfSeparationOffset/SinusoidalSeparatorFunction(thumbnailDeltaPosition / 2.0f);
				}
					break;
				case SeparatorMethod.LINEAR:
				{
					separatorMultiplier = halfSeparationOffset/LinearSeparatorFunction(thumbnailDeltaPosition / 2.0f);
				}
					break;
				case SeparatorMethod.SINUSOIDAL:
				{
					separatorMultiplier = halfSeparationOffset/SinusoidalSeparatorFunction(thumbnailDeltaPosition / 2.0f);
				}
					break;
			}
		}
		
		thumbnailScrollPosition = new float[numberOfThumbnails];
		thumbnailPositions = new Vector2[numberOfThumbnails];
		
		thumbnailList = new RectTransform[numberOfThumbnails];
		
		thumbnailObjectGroup.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (numberOfThumbnails-1f)*thumbnailSize.x*relativeStepDistance + thumbnailPanel.rect.width);
		
		for(int i = 0; i<numberOfThumbnails; i++) {
			thumbnailList[i] = Instantiate(thumbnailPrefab) as RectTransform;
			thumbnailList[i].transform.SetParent(thumbnailObjectGroup.transform, false);
			
			thumbnailList[i].name = thumbnails[i].name;
			
			thumbnailList[i].GetComponent<ThumbnailBehavior>().Initialize(thumbnails[i], this, i);
			
			thumbnailPositions[i] = Vector2.zero;
			
			thumbnailPositions[i].x =  thumbnailPanel.rect.width/2f + i*thumbnailSize.x*relativeStepDistance - thumbnailObjectGroup.rect.width/2f;
			
			thumbnailList[i].anchoredPosition = thumbnailPositions[i];
			
			thumbnailScrollPosition[i] = i*thumbnailDeltaPosition;
		}
		
		thumbnailIsSet = true;
		
		Vector3 tempPositoon = thumbnailObjectGroup.position;
		tempPositoon.x = -thumbnailPositions[0].x + thumbnailObjectGroup.rect.width/2f;
		
		thumbnailObjectGroup.position = tempPositoon;
		
		currentThumbnail = 0;
		lastCurrentThumbnail = 0;

		if(activateHighlight) {
			ToggleThumbnailHighlight(currentThumbnail, true);
		}

		FocusCurrentThumbnail();

		if(panelGroupWasDeactive) {
			DeactivatePanelGroup();
		}
	}

	private float LinearSeparatorFunction(float variable) {
		float functionRange = relativeStepSeparator * thumbnailDeltaPosition;

		if((variable < functionRange) && (variable > -functionRange)) {
			return variable / functionRange;
		}
		else if(variable >= functionRange) {
			return 1.0f;
		}
		else if(variable <= -functionRange) {
			return -1.0f;
		}
		else {
			Debug.Log("Imaginary number?");
			return 0;
		}
	}

	private float SinusoidalSeparatorFunction(float variable) {
		float functionRange = relativeStepSeparator * thumbnailDeltaPosition;
		
		if( (variable < functionRange) && (variable > -functionRange) ) {
			return Mathf.Sin(variable*(Mathf.PI/2f)/functionRange);
		}
		else if(variable >= functionRange){
			return 1.0f;
		}
		else if(variable <= -functionRange) {
			return -1.0f;
		}
		else {
			Debug.Log("Imaginary number?");
			return 0;
		}
	}

	private float QuadraticScaleFunction(float variable) {
		float functionRange = relativeStepScale * thumbnailDeltaPosition;

		if( (variable < functionRange) && (variable > -functionRange) ) {
			return 1f + (1f - thumbnailMaxScale)*(variable + functionRange)*(variable - functionRange)/(functionRange*functionRange);
		}
		else {
			return 1.0f;
		}
	}

	private float CosinusoidalScaleFunction(float variable) {
		float functionRange = relativeStepScale * thumbnailDeltaPosition;
		
		if( (variable < functionRange) && (variable > -functionRange) ) {
			return (thumbnailMaxScale - 1f)*Mathf.Cos(variable*(Mathf.PI/2f)/functionRange) + 1f;
		}
		else{
			return 1.0f;
		}
	}
	
	private float LinearScaleFunction(float variable) {
		float functionRange = relativeStepScale * thumbnailDeltaPosition;

		if((variable <= 0) && (variable > -functionRange)) {
			return (variable * (thumbnailMaxScale - 1f) / functionRange) + thumbnailMaxScale;
		}
		else if((variable > 0) && (variable < functionRange)) {
			return (variable * (1f - thumbnailMaxScale) / functionRange) + thumbnailMaxScale;
		}
		else {
			return 1.0f;
		}
	}

	public void UpdateThumbnails() {
		if(thumbnailIsSet) {	
			for(int i = 0; i<numberOfThumbnails; i++) {
				float scaleMultiplier = 1f;

				switch(scaleMethod) {
					case ScaleMethod.NONE:
					{
					}
						break;
					case ScaleMethod.LINEAR:
					{
						scaleMultiplier = LinearScaleFunction(thumbnailScrollPosition[i] - thumbnailScrollRect.horizontalNormalizedPosition);
					}
						break;
					case ScaleMethod.QUADRATIC:
					{
						scaleMultiplier = QuadraticScaleFunction(thumbnailScrollPosition[i] - thumbnailScrollRect.horizontalNormalizedPosition);
					}
						break;
					case ScaleMethod.COSINUSOIDAL:
					{
						scaleMultiplier = CosinusoidalScaleFunction(thumbnailScrollPosition[i] - thumbnailScrollRect.horizontalNormalizedPosition);
					}
						break;
				}

				thumbnailList[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, thumbnailSize.x*scaleMultiplier);
				thumbnailList[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, thumbnailSize.y*scaleMultiplier);

				Vector2 thumbnailOffset = Vector2.zero;
				switch(separatorMethod) {
					case SeparatorMethod.NONE:
					{
						if(useSmoothSwitch) {
							thumbnailOffset.x = separatorMultiplier*SinusoidalSeparatorFunction(thumbnailScrollPosition[i] - thumbnailScrollRect.horizontalNormalizedPosition);
							thumbnailList[i].anchoredPosition = thumbnailPositions[i] + thumbnailOffset;
						}
					}
						break;
					case SeparatorMethod.LINEAR:
					{
						thumbnailOffset.x = separatorMultiplier*LinearSeparatorFunction(thumbnailScrollPosition[i] - thumbnailScrollRect.horizontalNormalizedPosition);
						thumbnailList[i].anchoredPosition = thumbnailPositions[i] + thumbnailOffset;
					}
						break;
					case SeparatorMethod.SINUSOIDAL:
					{
						thumbnailOffset.x = separatorMultiplier*SinusoidalSeparatorFunction(thumbnailScrollPosition[i] - thumbnailScrollRect.horizontalNormalizedPosition);
						thumbnailList[i].anchoredPosition = thumbnailPositions[i] + thumbnailOffset;
					}
						break;
				}
			}

			CheckCurrentThumbnail();
		}
	}

	public void CheckCurrentThumbnail() {
		if(thumbnailIsSet){
			bool found = false;
			
			lastCurrentThumbnail = currentThumbnail;
			
			for(int i=0; i < numberOfThumbnails; i++) {
				if(found) {
					thumbnailList[i].SetSiblingIndex((numberOfThumbnails-1)-(i-currentThumbnail));
				}
				else if ( (thumbnailScrollPosition[i] - thumbnailScrollRect.horizontalNormalizedPosition >= -thumbnailDeltaPosition/2.0f) && (thumbnailScrollPosition[i] - thumbnailScrollRect.horizontalNormalizedPosition <= thumbnailDeltaPosition/2.0f) ) {
					currentThumbnail = i;
					thumbnailList[i].SetSiblingIndex(numberOfThumbnails-1);
					found = true;
					for (int l=0; l<i; l++) {
						thumbnailList[l].SetSiblingIndex(l);
					}
					break;
				}
			}
			
			if( (lastCurrentThumbnail != currentThumbnail) && (activateHighlight) ) {

				ToggleThumbnailHighlight(currentThumbnail, true);
				
				ToggleThumbnailHighlight(lastCurrentThumbnail, false);
			}

			if(isDragging) {
				if(currentThumbnail != thumbnailIndexOnBegginDrag) {
					triggerThumbnailEventAtEndOfFocus = true;
				}
				else {
					triggerThumbnailEventAtEndOfFocus = false;
				}
			}
		}
	}

	public void OnBeginDrag() {
		isDragging = true;
		checkInertia = false;
		thumbnailIndexOnBegginDrag = currentThumbnail;
	}

	public void OnEndDrag() {
		if(useInertia) {
			checkInertia = true;
		}
		else {
			isDragging = false;
			FocusCurrentThumbnail();
		}
	}

	private void ToggleThumbnailHighlight(int thumbnailIndex, bool turnOn) {
		UnityEngine.UI.Image[] objectList;
		objectList = thumbnailList[thumbnailIndex].GetComponentsInChildren<UnityEngine.UI.Image>(true);
		for(int i = 0; i<objectList.Length;i++) {
			if(objectList[i].name == "Highlight") {
				if(objectList[i].enabled != turnOn) {
					objectList[i].enabled = turnOn;
				}
			}
		}
	}

	public void FocusCurrentThumbnail () {
		if(thumbnailIsSet) {
			onFocusThumbnail = true;
			Hashtable args = iTween.Hash("from",thumbnailScrollRect.horizontalNormalizedPosition,"to",thumbnailScrollPosition[currentThumbnail],"time",focusPeriod,"onupdate","OnFocusCurrentThumbnail","oncomplete","OnEndFocusCurrentThumbnail","oncompleteparams", triggerThumbnailEventAtEndOfFocus);
			
			iTween.ValueTo(gameObject, args);
		}
	}

	public void FocusThumbnail (int thumbnailIndex, bool triggerObjectEventAtEnd = true) {
		if(thumbnailIsSet) {
			isDragging = false;
			onFocusThumbnail = true;
			Hashtable args = iTween.Hash("from",thumbnailScrollRect.horizontalNormalizedPosition,"to",thumbnailScrollPosition[thumbnailIndex],"time",focusPeriod,"onupdate","OnFocusCurrentThumbnail","oncomplete","OnEndFocusCurrentThumbnail","oncompleteparams", triggerObjectEventAtEnd);
			
			iTween.ValueTo(gameObject, args);
		}
	}

	private void OnFocusCurrentThumbnail(float newValue){
		thumbnailScrollRect.horizontalNormalizedPosition = newValue;
	}

	private void OnEndFocusCurrentThumbnail(bool triggerEvent) {
		onFocusThumbnail = false;
		if(triggerEvent) {
			triggerThumbnailEventAtEndOfFocus = false;
			thumbnailList[currentThumbnail].GetComponent<ThumbnailBehavior>().TriggerEvent();
		}
	}

	private bool DeactivatePanelGroup() {
		if(thumbnailObjectGroup.gameObject.activeSelf) {
			thumbnailObjectGroup.gameObject.SetActive(false);

			return true;
		}

		return false;
	}

	private bool ActivatePanelGroup() {
		if(!thumbnailObjectGroup.gameObject.activeSelf) {
			thumbnailObjectGroup.gameObject.SetActive(true);

			return true;
		}

		return false;
	}
}

