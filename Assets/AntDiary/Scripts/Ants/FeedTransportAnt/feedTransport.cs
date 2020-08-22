using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntDiary;
//using MoveAnt;
//using hogehoge;　//アリ情報の取得プログラム

//餌運びプログラム
//同じところを往復するプログラムを作成
//違うところに帰りたければ修正が必要
//
public class feedTransport : MonoBehaviour
{
    //MoveAntの取得
    AntMovement antMovement;

    //餌の有無
    public bool isHoldingFood = false;
    //目的地の定義
    public Vector2 targetFeed;
    public Vector2 targetNest;
    public Vector2 targetNestEntrance;
    public Vector2 target;
    public Vector2 from;

    //public Vector2 nowPlace;

    //AStarへのnodeの初期化
    　 //経路探索のプログラムにぶち込む為の操作
    　 //経路全体を示す変数名"nodeGraph"
       NodeGraph nodeGraph;

       //試験的に3つのnodoを作ってみる(実際には使わない)
       Node nodeNest;
	   Node nodeFeed;
	   Node nodeNestEntrance;


       //探索ごとにAStarSearcherインスタンスを作る
       //ここに経路探索の結果を入れる
       AStarSearcher searcher;

       // 探索結果をもとに曲がり角の座標を保存するリスト型配列
       public List<Vector2> v;
       //その引数i
    　　public int i;
        //リスト型配列の大きさI
        public int I=-1;

    //
    //対象までのスピード    public float speed;
    //対象までの距離
    public Vector2 distance ;

    //運搬能力
    //public int ability = 1;
    
    
    // Start is called before the first frame update
    
    void Start()
    {
        
        antMovement = new AntMovement();
       //目的地の設定

       //餌と巣の位置情報はテスト用(後で削除または変更する可能性あり)
       //餌の位置
       targetFeed = GameObject.Find("feed").transform.position;
       //巣の位置
       targetNest = GameObject.Find("nest").transform.position;
       //現在置
       //自身の座標を取得
       antMovement.nowPlace = transform.position;

       //経路探索のプログラムにぶち込むためのnode作成
       nodeGraph = new NodeGraph();
       nodeNest = nodeGraph.CreateNode(targetNest);
	   nodeFeed = nodeGraph.CreateNode(targetFeed);
       //現在は曲がり角を生成していないため、nodeは2つだけ
       //試しにtargetNestEntrance（巣の出入り口）なるものを曲がり角として用意してみる
       targetNestEntrance = GameObject.Find("nestEntrance").transform.position;
       nodeNestEntrance = nodeGraph.CreateNode(targetNestEntrance);
       //nodeをそれぞれつなげる作業
       //nodeGraph.ConnectNodes(new []{from,to});→ fromからtoへnodeをつなげる
       //nodeGraph.ConnectNodes(new []{A, B,C});→ABCの循環円を作成
       nodeGraph.ConnectNodes(new []{nodeNest, nodeNestEntrance});
       nodeGraph.ConnectNodes(new []{nodeNestEntrance,nodeFeed});
       //探索ごとにAStarSearcherインスタンスを作る
       searcher = new AStarSearcher(nodeGraph);
       //経路探索をする
	   searcher.SearchRoute(nodeNest, nodeFeed);
       Debug.Log(searcher.Route);
       //結果はsearcher.Routeに入る
		foreach(var node in searcher.Route){
            //vにnode.Posで値を取得する
            v.Add(node.WorldPosition);
			//Debug.Log(node.ToString());
            I++; //vの大きさ
		}


       //ありの状態の取得
       //初期位置
       antMovement.nowPlace = transform.position;
       //アリのスピード取得(未取得)
       //public float speed = hogehoge.speed;
       //試験用スピード //これは後で削除
       antMovement.speed = 0.01f;
       //node間の移動を計算し足していく値
       //最初は1番目の目的地へと向かう
       i = 1;
       target = v[i];
       antMovement.way = antMovement.Explore(target);

    }



    // Update is called once per frame
    void Update()
    {
        //現在地を獲得
        antMovement.nowPlace = transform.position;
        //アリの現在地がtarget周辺についたら
        if(antMovement.isArrived(target) ){
           //餌を持ってなければ
           if(isHoldingFood == false){
               //餌場じゃなければ               
            if(target != v[I]){ 
                //次の目的地へ移動させる
                i++;
                target = v[i];

                antMovement.way = antMovement.Explore(target);
                // gotoTarget();
            }else{//餌場ならば
             //餌をゲットする
             getFeed();
            }
           }else{ //餌を持っているなら
           //目的地が巣ではないのなら
             if(target != v[0]){
                //巣へ近づけさせる
                i--;
                target = v[i];
                antMovement.way = antMovement.Explore(target);
                }else{//巣に到着したら
                //餌をおく
                leaveFeed();
               }
           }   
        }
        //Explore()の結果に従い移動する
        antMovement.nowPlace += antMovement.way;
        transform.position = antMovement.nowPlace;
    }


    //餌を獲得
    public void getFeed(){
        //巣へ帰還する道の探索
        from = v[i];
        i--;
        target = v[i];
        antMovement.way = antMovement.Explore(target);
        //経路探索をする
        //餌をゲットした状態にする
        isHoldingFood = true;
    }

    //餌を離す
    public void leaveFeed(){
        //目的地を餌に変更
        i++;
        target=v[i];
        antMovement.way = antMovement.Explore(target);
        //経路探索をする
        //餌を持ってない状態にする
        isHoldingFood = false;
    }

}
