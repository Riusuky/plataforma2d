using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ThumbnailBehavior : MonoBehaviour
{
	public UnityEngine.UI.Image image;

	private ThumbnailScroll thumbnailScroll;
	private string name;
	private string URL;
	private int index;

	public void Initialize(ThumbnailStruct thumbnailStruct, ThumbnailScroll _thumbnailScroll, int _index) {
		name = thumbnailStruct.name;
		image.sprite = thumbnailStruct.sprite;
		URL = thumbnailStruct.URL;
		thumbnailScroll = _thumbnailScroll;
		index = _index;

		if(GetComponent<Button>() != null) {
			GetComponent<Button>().onClick.AddListener(() => thumbnailScroll.FocusThumbnail(index));
		}
	}

	public void TriggerEvent() {
		Debug.Log("Event triggered. ("+name+")");
	}
}

