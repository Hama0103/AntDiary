﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntMovement : MonoBehaviour
{
    //アリの現在の位置
    public Vector2 nowPlace;
    //アリのスピード(比例定数的に処理)
    public float speed;
    //実際に足される値

    public Vector2 way;
    // Start is called before the first frame update
    //

    public AntMovement(){
        nowPlace = new Vector2(0,0);
        speed = 0.01f;
        way = new Vector2(0,0);
    }
    public Vector2 Explore(Vector2 target){
        //対象までの距離
       Vector2 distance = target - nowPlace;
       //対象までの進む道(この場合スピードと同義) これを足していく
       Vector2 way = new Vector2(0,0);
       way.x = speed* distance.x/distance.magnitude;
       way.y = speed* distance.y/distance.magnitude;
       return way;
    }
}
