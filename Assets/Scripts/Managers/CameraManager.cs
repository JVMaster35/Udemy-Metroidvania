using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public BoxCollider2D enviromentBox;

    private Player player;
    private float halfHeight;
    private float halfWidth;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayLevelMusic();

        player = FindObjectOfType<Player>();

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, enviromentBox.bounds.min.x + halfWidth, enviromentBox.bounds.max.x - halfWidth),
                Mathf.Clamp(player.transform.position.y, enviromentBox.bounds.min.y + halfHeight, enviromentBox.bounds.max.y - halfHeight), transform.position.z);
        }
    }
}
