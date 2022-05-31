using Assets;
using Assets.Flower;
using UnityEngine;

public class UserInteractionManager : MonoBehaviour
{
    void Update()
    {
        if (!ServiceLocator.Instance.Flower.IsDead)
        {  
            //for Android
            for (int i = 0; i < Input.touchCount; ++i)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    // Construct a ray from the current touch coordinates
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);

                    if (Physics.Raycast(ray, out var hit) && hit.transform.TryGetComponent(out Flower _))
                    {
                        ServiceLocator.Instance.Flower.Agreement();
                    }
                }
            }

            //For PC
            if (Input.GetMouseButtonUp(0))
            {
                // Construct a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit) && hit.transform.TryGetComponent(out Flower _))
                {
                    ServiceLocator.Instance.Flower.Agreement();
                }
            }
        }
    }
}
