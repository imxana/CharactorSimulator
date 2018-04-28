using UnityEngine;

[CreateAssetMenu(menuName = "Item Object")]
public class ItemObject : ScriptableObject
{


	public GameObject objectPrefab;
	public Sprite objectImage;
	public int quantity = 0;
	public ItemObjectLogic itemLogic;
	public string objectTooltip;

}