using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayerSystem : MonoBehaviour
{
    public float FollowSpeed = 3f;

    private Transform _enemyBody;
    private void Start()
    {
        _enemyBody = transform.Find("EnemySpriteContainer");
    }
    public void MoveToTarget(Transform targetTransform)
    {
        if(targetTransform.position.y > this.transform.position.y)
        {
            //Go up
            this.transform.position += Vector3.up * FollowSpeed * Time.deltaTime;
        }
        else if(targetTransform.position.y < this.transform.position.y)
        {
            //Go down
            this.transform.position -= Vector3.up * FollowSpeed * Time.deltaTime;
        }
        if(Mathf.Abs(targetTransform.position.y - this.transform.position.y) < 1)
        {
            if(targetTransform.position.x < this.transform.position.x)
            {
                //Go left
                this.transform.position += Vector3.left * FollowSpeed * Time.deltaTime;
                if(_enemyBody.rotation.y>0)
                    _enemyBody.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                //Go right
                this.transform.position += Vector3.right * FollowSpeed * Time.deltaTime;
                _enemyBody.rotation = Quaternion.Euler(0, 180, 0);
            }
            return;
        }
    }
}
