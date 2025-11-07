using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoverService : MonoBehaviour
{
    // Класс для перемещения объектов в кат-сценах

    private float overshootAmount;

    private void Start()
    {
        overshootAmount = 0.22f;
    }

    public void AnimateMoving(GameObject targetObject, Transform destination, ObjectAnimationType animationType, bool isNeedToAnimateScale, float animationDuration = 1f)
    {
        if (targetObject == null)
        {
            Debug.LogError("targetObject is null!");
            return;
        }

        if (destination == null)
        {
            Debug.LogError("destination is null!");
            return;
        }

        if (!enabled || !gameObject.activeInHierarchy)
        {
            Debug.LogError("ObjectMover или его GameObject неактивен!");
            return;
        }

        switch (animationType)
        {
            case ObjectAnimationType.Linear:
                StartCoroutine(LinearMovement(targetObject, destination, isNeedToAnimateScale, animationDuration));
                break;
            case ObjectAnimationType.EaseIn:
                StartCoroutine(EaseInMovement(targetObject, destination, isNeedToAnimateScale, animationDuration));
                break;
            case ObjectAnimationType.EaseOut:
                StartCoroutine(EaseOutMovement(targetObject, destination, isNeedToAnimateScale, animationDuration));
                break;
            case ObjectAnimationType.Overshoot:
                StartCoroutine(OvershootMovement(targetObject, destination, isNeedToAnimateScale, animationDuration));
                break;
        }
    }

    private IEnumerator LinearMovement(GameObject targetObject, Transform destination, bool animateScale, float animationDuration)
    {
        Vector3 startPosition = targetObject.transform.position;
        Vector3 endPosition = destination.position;
        Vector3 startScale = targetObject.transform.localScale;
        Vector3 endScale = destination.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;

            targetObject.transform.position = Vector3.Lerp(startPosition, endPosition, t);

            if (animateScale)
            {
                targetObject.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            }

            yield return null;
        }

        targetObject.transform.position = endPosition;
        if (animateScale)
        {
            targetObject.transform.localScale = endScale;
        }
    }

    private IEnumerator EaseInMovement(GameObject targetObject, Transform destination, bool animateScale, float animationDuration)
    {
        Vector3 startPosition = targetObject.transform.position;
        Vector3 endPosition = destination.position;
        Vector3 startScale = targetObject.transform.localScale;
        Vector3 endScale = destination.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            float easedT = t * t;

            targetObject.transform.position = Vector3.Lerp(startPosition, endPosition, easedT);

            if (animateScale)
            {
                targetObject.transform.localScale = Vector3.Lerp(startScale, endScale, easedT);
            }

            yield return null;
        }

        targetObject.transform.position = endPosition;
        if (animateScale)
        {
            targetObject.transform.localScale = endScale;
        }
    }

    private IEnumerator EaseOutMovement(GameObject targetObject, Transform destination, bool animateScale, float animationDuration)
    {
        Vector3 startPosition = targetObject.transform.position;
        Vector3 endPosition = destination.position;
        Vector3 startScale = targetObject.transform.localScale;
        Vector3 endScale = destination.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            float easedT = 1f - (1f - t) * (1f - t);

            targetObject.transform.position = Vector3.Lerp(startPosition, endPosition, easedT);

            if (animateScale)
            {
                targetObject.transform.localScale = Vector3.Lerp(startScale, endScale, easedT);
            }

            yield return null;
        }

        targetObject.transform.position = endPosition;
        if (animateScale)
        {
            targetObject.transform.localScale = endScale;
        }
    }

    private IEnumerator OvershootMovement(GameObject targetObject, Transform destination, bool animateScale, float animationDuration)
    {
        Vector3 startPosition = targetObject.transform.position;
        Vector3 endPosition = destination.position;
        Vector3 startScale = targetObject.transform.localScale;
        Vector3 endScale = destination.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            float overshootT = t * t * ((overshootAmount + 1f) * t - overshootAmount);

            targetObject.transform.position = Vector3.Lerp(startPosition, endPosition, overshootT);

            if (animateScale)
            {
                targetObject.transform.localScale = Vector3.Lerp(startScale, endScale, overshootT);
            }

            yield return null;
        }

        targetObject.transform.position = endPosition;
        if (animateScale)
        {
            targetObject.transform.localScale = endScale;
        }
    }
}

public enum ObjectAnimationType
{
    Linear,
    EaseIn,
    EaseOut,
    Overshoot
}