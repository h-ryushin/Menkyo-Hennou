using UnityEngine;

public class CarLoop : MonoBehaviour
{
    public float speed = 5f;         // 移動スピード
    public float limitX = 10f;       // 画面外の判定（x座標の限界）

    private Vector3 startPos;        // 最初の位置（ループ用）

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // x方向に移動
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // 右に行きすぎたら左にワープ
        if (transform.position.x > limitX)
        {
            transform.position = new Vector3(-limitX, transform.position.y, transform.position.z);
        }

        // 左に行きすぎたら右にワープ
        if (transform.position.x < -limitX)
        {
            transform.position = new Vector3(limitX, transform.position.y, transform.position.z);
        }
    }
}