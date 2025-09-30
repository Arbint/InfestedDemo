using UnityEngine;

public class WidgetComponent : MonoBehaviour
{
    [SerializeField] UserWidget mWidgetPrefab;
    [SerializeField] Transform mWidgetAttachTransform;

    UserWidget mWidget;
    static Canvas mMainCanvas;
    private void Awake()
    {
        mWidget = Instantiate(mWidgetPrefab);
        mWidget.SetOwner(gameObject);
    }

    private void Start()
    {
       if(!mMainCanvas)
       {
           mMainCanvas = FindFirstObjectByType<Canvas>(); 
       }

       if(mMainCanvas)
       {
            mWidget.transform.SetParent(mMainCanvas.transform);
       }
    }

    private void Update()
    {
        if(mWidget)
        {
            Vector3 windgetPos = Camera.main.WorldToScreenPoint(mWidgetAttachTransform.position);            
            mWidget.transform.position = windgetPos;
        }
    }
}
