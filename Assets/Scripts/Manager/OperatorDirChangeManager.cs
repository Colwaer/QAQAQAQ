using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;
using Map;

public class OperatorDirChangeManager : Singleton<OperatorDirChangeManager>
{
    LayerMask operatorLayer;

    Camera mainCamera;
    Vector3 originCameraPos;
    Vector3 targetCameraPos;

    int xMax;
    int yMax;

    bool isInBattleState;
    bool isChoosingDir;

    public GameObject dirChoosePanelPre;
    GameObject curDirChoosePanel;

    BaseOperator baseOperator;

    Vector2 MousePosition;
    Vector2 curOperatorPos;
    public bool CanInteractWithOperator { get; set; }
    

    private void Start()
    {

        mainCamera = Camera.main;

        originCameraPos = mainCamera.transform.position;

        Physics2D.queriesStartInColliders = false;
        operatorLayer = LayerMask.GetMask("Operator");
        
        Gizmos.color = Color.red;
    }

    private void Update()
    {
        MousePosition = PlaceManager.Instance.MousePosition;
        if (Input.GetKeyDown(KeyCode.S))
            EnterBattleState();
        if (!IsMousePosInMap())
            return;
        if (isChoosingDir)
        {
            if (Input.GetMouseButtonDown(0))
            {
                
                ChangeDir();
                EndChoose();
            }
        }
        if (isInBattleState && !isChoosingDir)
        {           
            if (Input.GetMouseButtonDown(0))
            {
                RayCast();
            }            
        }
        

    }

    private void OnDrawGizmos()
    {
        
    }
    void RayCast()
    {

        

        curOperatorPos = MousePosition;
        operatorLayer = LayerMask.GetMask("Operator");
        //Debug.Log("enterbattlestate");
        RaycastHit2D hit = Physics2D.Raycast(MousePosition - new Vector2(0.5f, 0), new Vector2(1, 0), 0.9f, operatorLayer);
        
        if (hit.collider != null && hit.collider.tag == "Operator")
        {
            CameraTransform();

            baseOperator = hit.collider.GetComponent<BaseOperator>();
            Time.timeScale = 0.1f;

            ShowDirChoosePanel();
            //Debug.Log(hit.collider.gameObject);
            baseOperator.ShowDirChoosePanel();
            isChoosingDir = true;
        }
    }
    void CameraTransform()
    {
        //mainCamera.transform.position = new Vector3(MousePosition.x, MousePosition.y, -10);
        //StartCoroutine(CamerMove());
        mainCamera.orthographicSize = 4.8f;
    }
    //IEnumerator CamerMove()
    //{
        /*
        while (Vector2.Distance(mainCamera.transform.position, targetCameraPos) > 0.05f)
        {
            Debug.Log(Vector2.Distance((Vector2)mainCamera.transform.position, (Vector2)targetCameraPos));
            yield return new WaitForSeconds(0.5f);
            mainCamera.orthographicSize = Mathf.Lerp(5, 4.1f, 0.5f);
            mainCamera.transform.position = Vector2.Lerp(originCameraPos, targetCameraPos, 0.5f);
            mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
        */
    //}
    void CameraReset()
    {
        mainCamera.transform.position = originCameraPos;
        mainCamera.orthographicSize = 5f;
    }
    void EndChoose()
    {
        CameraReset();

        OffShowDirChoosePanel();
        baseOperator.OffShowDirChoosePanel();
        Time.timeScale = 1.0f;
        isChoosingDir = false;
    }
    void ChangeDir()
    {
        Vector2 lookDirection;
        PlaceDirection dir;
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - baseOperator.transform.position;
        float angle = 90 - Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg;
        angle = Mathf.Deg2Rad * angle;
        //Debug.LogFormat("Cos:{0}, Sin:{1}", Mathf.Cos(angle), Mathf.Sin(angle));
        //Debug.Log(angle);
        if (Mathf.Cos(angle) >= 0.7f && Mathf.Abs(Mathf.Sin(angle)) <= 0.7f)
        {
            dir = PlaceDirection.right;
            

        }
        else if (Mathf.Abs(Mathf.Cos(angle)) <= 0.7f && Mathf.Sin(angle) >= 0.7f)
        {
            dir = PlaceDirection.up;

        }
        else if (Mathf.Cos(angle) <= -0.7f && Mathf.Abs(Mathf.Sin(angle)) <= 0.7f)
        {
            dir = PlaceDirection.left;

        }
        else
        {
            dir = PlaceDirection.down;
        }
        //Debug.Log(dir);
        baseOperator.Init(dir);
    }

    public void ShowDirChoosePanel()
    {
        curDirChoosePanel = Instantiate(dirChoosePanelPre);
        curDirChoosePanel.transform.position = MousePosition;
    }
    public void OffShowDirChoosePanel()
    {
        if (curDirChoosePanel != null)
            Destroy(curDirChoosePanel);
    }
    public void EnterBattleState()
    {
        xMax = MapInSceneManager.Instance.xMax;
        yMax = MapInSceneManager.Instance.yMax;

        PlaceManager.Instance.IsPlacingOperator = false;
        MapCreate.Instance.ForbidAllBools();
        isInBattleState = true;
    }
    bool IsMousePosInMap()
    {
        if (MousePosition.x >= xMax || MousePosition.x < 0 || MousePosition.y >= yMax || MousePosition.y < 0)
            return false;
        return true;
    }
}
