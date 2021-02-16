using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownLayering : MonoBehaviour {
    public int orderInLayer;
    SpriteRenderer sp;

    // Start is called before the first frame update
    void Start() {
        sp = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        sp.sortingOrder = (int)(-8 * (orderInLayer + transform.position.y));
    }
}
