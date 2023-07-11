using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



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
                //Rotation(DrawType.Back);
                break;

            case DrawType.Back:
                StartCoroutine(Rotate(DrawType.Front));
                //Rotation(DrawType.Front);
                break;
        }
    }

    private void Rotation(DrawType type)
    {
        drawing = true;

        float targetY = (type == DrawType.Front) ? 180f : 0f;
        float rotateDuration = 0.5f;

        collider2D.enabled = false;

        transform.DORotate(new Vector3(0f, targetY, 0f), rotateDuration)
            .OnComplete(() =>
            {
                drawType = type;
                collider2D.enabled = type == DrawType.Front;
                drawing = false;
            });
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

        drawing = false;
    }

    private void OnEnable()
    {
        StopAllCoroutines();

        drawType = DrawType.Back;

        transform.rotation = Quaternion.Euler(0f, -180f, 0f);

        drawing = false;

        collider2D.enabled = true;
    }

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
    }
    
}