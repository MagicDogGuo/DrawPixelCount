using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//infoRef: http://www.iverv.com/2014/04/unitypixels.html

public class Percent : MonoBehaviour
{
    public Camera myCamera; //掛著前景Camera

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("GetPercent");
        }
    }

    public IEnumerator GetPercent()
    {
        //先擷取所有物件的畫面
        yield return new WaitForEndOfFrame();
        Texture2D texF = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        texF.ReadPixels(new Rect(0, 0, texF.width, texF.height), 0, 0, false);
        texF.Apply();


        //接著先Culling掉目標Camera，然後擷取畫面取得背景圖片
        int PrevMask = myCamera.cullingMask;
        myCamera.cullingMask = 0;
        yield return new WaitForEndOfFrame();

        Texture2D texB = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        texB.ReadPixels(new Rect(0, 0, texB.width, texB.height), 0, 0, false);
        texB.Apply();

        myCamera.cullingMask = PrevMask;
        myCamera.Render(); //強制Render一次避免產生閃爍


        //取得並判斷Pixel是否相同
        Color32[] colB = texB.GetPixels32();
        Color32[] colF = texF.GetPixels32();

        //判斷RGB的數值
        print("colB_R"+ colB[0].r+ " colB_G" + colB[0].g+ " colB_B" + colB[0].b);

        Destroy(texB);
        Destroy(texF);
        int count = 0;
        for (int i = 0; i < colF.Length; i++)
        {
            if (!Color32.Equals(colB[i], colF[i]))
                count++;
        }

        //計算百分比
        float percent = (float)count * 100 / (float)(Screen.width * Screen.height);
        Debug.Log("COLOR:" + percent);
    }
}

//找特定的顏色
//pixel的RGB都有對應到就+1
//藉著再除以pixel的總數量，就能得到該顏色的比例