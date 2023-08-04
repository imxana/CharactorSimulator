using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomItemGenerator : MonoBehaviour {

	public GameObject templateRock;    // 不可使用、不可叠加物品
    public GameObject templateCan; // 不可使用、可叠加物品
    public GameObject templateAid;  // 可使用、可叠加物品
    public Transform generatorPlane;

	protected Vector2 horizantolLimit = new Vector2(-2, 2);
	protected Vector2 verticleLimit = new Vector2(1, 5);

    public void Start()
	{
		templateRock.SetActive(false);
        templateCan.SetActive(false);
        templateAid.SetActive(false);

        InitializeItems();
    }

    public void InitializeItems()
    {
        InitializeItems(templateRock);
        InitializeItems(templateCan, 6);
        InitializeItems(templateAid);
    }

    public void ResetItems()
    {
        ResetItems(templateRock);
        ResetItems(templateCan, 6);
        ResetItems(templateAid);
    }

    private void RandSetObject(GameObject obj)
    {
        float randX = Random.Range(horizantolLimit.x, horizantolLimit.y);
        float randZ = Random.Range(verticleLimit.x, verticleLimit.y);
        obj.transform.position = new Vector3(randX, 0.3f, randZ);
        obj.SetActive(true);
    }

    private void InitializeItems(GameObject tempObj, int num = 3)
	{
        for (int i = 0; i < num; i++)
        {
            GameObject obj;
            string objName = tempObj.name + i;
            Transform objTrans = generatorPlane.Find(objName);
            if (objTrans != null)
            {
                obj = objTrans.gameObject;
            }
            else
            {
                obj = Instantiate(tempObj, generatorPlane);

            }
            RandSetObject(obj);
            obj.name = tempObj.name + i;
        }
    }

    private void ResetItems(GameObject tempObj, int num = 3)
    {
        for (int i = 0; i < num; i++)
        {
            string objName = tempObj.name + i;
            Transform objTrans = generatorPlane.Find(objName);
            if (objTrans != null)
            {
                RandSetObject(objTrans.gameObject);
            }

        }
    }
}
