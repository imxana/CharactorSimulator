using UnityEngine;

public enum ItemType
{
	Material,
	Consumables
}

[System.Serializable]
public class ItemObjectLogic
{
	public ItemType itemType;

	public void UseItem(Transform tf, ItemObject item)
	{
		switch (itemType) 
		{
		case ItemType.Material:
//			DiscardItem (tf, item);
			RandomPutMaterial (tf, item);
			break;
		case ItemType.Consumables:
			UseConsumables ();
			break;
		default:
			break;
		}
	}

	// throw item to a random angel
	public void DiscardItem(Transform tf, ItemObject item, int OneTimeDiscard = 1)
	{
		for (int i = 0; i < OneTimeDiscard; i++)
		{
			Vector3 RandomV3 = Random.onUnitSphere;
			RandomV3.y = Mathf.Abs (RandomV3.y);
			Vector3 pos = tf.position + tf.up / 2 + RandomV3 * 2 / 3;

			GameObject clone = Object.Instantiate (item.objectPrefab, pos, Quaternion.identity);
			clone.GetComponent<Rigidbody> ().velocity = RandomV3;
		}
	}

	// random put mat in the plane
	private void RandomPutMaterial(Transform tf, ItemObject item)
	{
		int f = Random.Range (-10, 10); 
		Vector3 pos = tf.position
			+ tf.right * f / 20
			+ tf.forward * (20 - Mathf.Abs( f )) / 30
			+ tf.up * 1 / 4;
		Object.Instantiate (item.objectPrefab, pos, Quaternion.identity);
	}
	// todo with consumables
	private void UseConsumables()
	{
		
	}
}
