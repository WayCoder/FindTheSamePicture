using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Card : MonoBehaviour
{
    public enum DrawType
    {
        Front,
        Back,
    }

    [field: SerializeField] public int index { get; private set; }


    private new Collider2D collider2D;

    [field: SerializeField] public DrawType drawType { get; private set; }

    public bool drawing { get; private set; }


    public void Init()
    {
        drawType = DrawType.Back;


        StopAllCoroutines();

    }
    public void Draw()
    {
        if (drawing)
        {
            return;
        }

        switch (drawType)
        {
            case DrawType.Front:
               StartCoroutine(Rotate(DrawType.Back));
                break;

            case DrawType.Back:
                StartCoroutine(Rotate(DrawType.Front));
                break;
        }
    }
    private IEnumerator Rotate(DrawType type)
    {
        drawing = true;

        float targetY = 180f;

        float totalRotation = 0f;

        float rotateSpeed = GameManager.instance.data.cardData.rotateSpeed;

        collider2D.enabled = false;

        while (totalRotation < Mathf.Abs(targetY))
        {
            float rotationAmount = rotateSpeed * Time.deltaTime;

            float remainingRotation = Mathf.Abs(targetY) - totalRotation;

            if (rotationAmount > remainingRotation)
            {
                rotationAmount = remainingRotation;
            }

            transform.Rotate(0f, rotationAmount, 0f);

            totalRotation += Mathf.Abs(rotationAmount);

            yield return null;
        }

        drawType = type;

        collider2D.enabled = type == DrawType.Front ? false : true;

        if(collider2D.enabled)
        {

        }

        drawing = false;
    }
    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
    }
    
}