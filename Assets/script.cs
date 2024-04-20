using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour
{
    public Rect screenRect;


    void LateUpdate()
    {
        // Texture2D 객체 생성
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        // 스크린의 색상 정보를 Texture2D로 읽어옴
        screenTexture.ReadPixels(screenRect, 0, 0);
        //screenTexture.Apply(); // 필요한 경우 Apply()를 호출하여 변경 사항을 적용

        // 특정 좌표에서 픽셀 색상값 변경
        for(int i=30; i<60; i++)
        screenTexture.SetPixel(i, i, Color.red); // 예시: (100, 100) 위치의 픽셀 색상을 빨간색으로 변경

        // 변경된 색상값을 화면에 적용
        screenTexture.Apply();
    }
}