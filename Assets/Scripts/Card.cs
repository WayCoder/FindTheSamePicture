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

    [field: SerializeField] public bool drawing { get; private set; }


    

    public void Draw()
    {
        if (drawing)
        {
            return;
        }

        drawing = true;

        collider2D.enabled = false;

        //Rotation(drawType == DrawType.Front ? DrawType.Back : DrawType.Front);
        StartCoroutine(Rotate(drawType == DrawType.Front ? DrawType.Back : DrawType.Front));

      
    }

    public void Flip()
    {

        drawType = drawType == DrawType.Front ? DrawType.Back : DrawType.Front;

        collider2D.enabled = drawType == DrawType.Front ? false : true;

        float targetY = (drawType == DrawType.Front) ? 180f : 0f;

        float rotateDuration = 0.5f;


        transform.DORotate(new Vector3(0f, targetY, 0f), rotateDuration);
          
    }

    private void Rotation(DrawType type)
    {
        float targetY = (type == DrawType.Front) ? 180f : 0f;

        float rotateDuration = 0.5f;


        transform.DORotate(Vector3.up, rotateDuration).SetEase(Ease.OutElastic)
            .OnComplete(() =>
            {
                drawType = type;

                collider2D.enabled = type == DrawType.Back;

                drawing = false;
            });
    }

    private IEnumerator Rotate(DrawType type)
    {       
        float targetY = 180f;

        float totalRotation = 0f;

        float rotateSpeed = GameManager.instance.data.cardData.rotateSpeed;

       
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