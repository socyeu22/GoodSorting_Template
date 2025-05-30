using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

// Lớp chịu trách nhiệm khởi tạo bàn chơi dựa trên dữ liệu level
// và quản lý các thao tác xếp hàng hóa trên kệ
public class BoardGenerator : MonoBehaviour
{
    public Transform shelfRoot;

    public List<ShelfController> shelfControllerList;

    public List<ShelfController> singlePushControllerList;

    public List<GameObject> waresPrefabList;

    public LayerMask shelfLayerMask;

    public LayerMask waresLayerMask;

    public WaresController currentSelectWares;

    protected ShelfController currentTargetShelf;

    private Vector3 mousePos;

    private Vector3 mouseWorldPos;

    private float focusInZ;

    public int[] productIDList;

    public int productPairCount = 24;

    public int shelfCount;

    public int singlePushCount;

    [HideInInspector]
    public int[] productPairCountList;

    [HideInInspector]
    public List<ProductPool> productPoolList = new List<ProductPool>();

    public List<ShelfTile> originalTileList = new List<ShelfTile>();

    public List<int> singlePushTileList = new List<int>();

    [HideInInspector]
    public List<int> emptySlotList = new List<int>();

    [HideInInspector]
    List<ProductPool> tempPool = new List<ProductPool>();

    [HideInInspector]
    public List<ShelfCell> cellListData = new List<ShelfCell>();

    [HideInInspector]
    public int positivePair;

    public LevelData currentLv;

    public ProductSet productSetData;

    public int shelfDeepMax;

  

    // Hàm khởi tạo MonoBehaviour, hiện chưa cần xử lý gì thêm
    void Start()
    {


    }

    // Hàm chính để tạo bàn chơi dựa trên dữ liệu level hiện tại
    // Bao gồm load scene, sản phẩm và sinh map sắp xếp trên các kệ
    public void GenBoard()
    {
        //Debug.Log("Scene ID " + currentLv.sceneID);
        LoadScene();
        LoadProduct();

        GenMap();
        GetSelfContainerList();
        GenerateBoard();
    }

    // Dọn dẹp dữ liệu bàn chơi cũ trước khi tạo bàn mới
    public void CleanBoard()
    {
        Destroy(transform.GetChild(0).gameObject);
        shelfControllerList.Clear();
        singlePushControllerList.Clear();
        waresPrefabList.Clear();
        productIDList = null;
        productPairCountList = null;
        productPoolList.Clear();
        originalTileList.Clear();
        singlePushTileList.Clear();
        emptySlotList.Clear();
        tempPool.Clear();
        cellListData.Clear();
    }


    // Tải scene chứa hệ thống kệ và tính toán số kệ cần thiết
    private void LoadScene()
    {
        //Resources.Load("LevelSO/" + "Level" + GameManager.instance.currentLevelIndex.ToString()) as LevelConfig;
        currentLv = Resources.Load("GameLevels/" + "Level" + GameManager.Instance.currentLevel.ToString()) as LevelData;
        GameObject sceneObj = Instantiate(Resources.Load("Shelves/" + currentLv.sceneID + "/Scene" + currentLv.sceneID) as GameObject);
        sceneObj.transform.parent = transform;
        shelfRoot = sceneObj.transform;

        // Đếm số kệ thường và kệ đơn trong scene vừa tải
        int shelfCountTemp = 0;

        int singlePushCountTemp = 0;

        for (int i = 0; i < shelfRoot.GetChild(0).childCount; i++)
        {
            // Kiểm tra loại kệ để xác định số lượng kệ ba ô và kệ đơn
            ShelfController shelfController = shelfRoot.GetChild(0).GetChild(i).GetComponent<ShelfController>();
            if (shelfController.type == ShelfController.ShelfType.TRIPLE)
                shelfCountTemp++;
            else if (shelfController.type == ShelfController.ShelfType.SINGLE)
                singlePushCountTemp++;
        }

        //shelfCount = shelfControllerList.Count;
        shelfCount = shelfCountTemp;
        singlePushCount = singlePushCountTemp;
        shelfDeepMax = currentLv.singlePush;
        Debug.Log("Shelf Count " + shelfCount);
        Debug.Log("Single Push Count " + singlePushCount);
       
    }

    // Đọc dữ liệu hàng hóa cần sử dụng cho level và tạo danh sách prefab
    private void LoadProduct()
    {
        Debug.Log("Product Set " + currentLv.dynamicProductSet);

        int productIndex = 0;

        // Nếu dynamicProductSet = 100000 nghĩa là level lựa chọn ngẫu nhiên bộ sản phẩm
        if (currentLv.dynamicProductSet == 100000)
        {
            productIndex = Random.Range(1, productSetData.productSetList.Count);
        }

        else
        {
            // Tìm bộ sản phẩm phù hợp với ID chỉ định trong level
            for (int i = 0; i < productSetData.productSetList.Count; i++)
            {
                if (currentLv.dynamicProductSet == productSetData.productSetList[i].ID)
                {
                    productIndex = i;
                    break;
                }

            }
        }

       

        Debug.Log("Product Set Count " + productSetData.productSetList[productIndex].productList.Count);

        productIDList = new int[productSetData.productSetList[productIndex].productList.Count];

        for (int i = 0; i < productSetData.productSetList[productIndex].productList.Count; i++)
        {
            productIDList[i] = productSetData.productSetList[productIndex].productList[i];
        }

        for (int i = 0; i < productSetData.productSetList[productIndex].productList.Count; i++)
        {
            int waresId = productSetData.productSetList[productIndex].productList[i];
            GameObject waresPrefab = Resources.Load("Wares/" + waresId + "/" + waresId) as GameObject;
            waresPrefabList.Add(waresPrefab);
        }

        productPairCount = currentLv.totalProductCount;

        // Kiểm tra tổng số ô chứa trên tất cả kệ có đủ để đặt hết sản phẩm hay không
        // Nếu chưa đủ sẽ tăng chiều sâu của kệ (shelfDeepMax) cho tới khi đáp ứng
        if (shelfCount * shelfDeepMax < (productPairCount + shelfCount))
        {
            Debug.Log("Must Fix Shelf Count");
            Debug.LogError("Fixed Shelf Count --- 1 time");
            shelfDeepMax++;
        }

        // Kiểm tra lại nhiều lần vì số ô có thể vẫn chưa đủ sau khi tăng
        if (shelfCount * shelfDeepMax < (productPairCount + shelfCount))
        {
            Debug.Log("Must Fix Shelf Count");
            Debug.LogError("Fixed Shelf Count --- 2 time");
            shelfDeepMax++;
        }

        // Lần kiểm tra cuối cùng, nếu vẫn thiếu sẽ log lỗi để nhà thiết kế xem lại dữ liệu level
        if (shelfCount * shelfDeepMax < (productPairCount + shelfCount))
        {
            Debug.Log("Must Fix Shelf Count");
            Debug.LogError("Fixed Shelf Count --- 3 time");
            shelfDeepMax++;
        }

        Debug.Log("Max shelf count " + shelfDeepMax);

        if (shelfCount * shelfDeepMax < (productPairCount + shelfCount))
        {
       
            Debug.LogError("Level Error --- product pair count is too big");
           
        }
    }

    // Lắng nghe thao tác kéo thả hàng hóa của người chơi
    // Chỉ xử lý khi đang ở trạng thái IN_GAME
    void Update()
    {
        if (GameManager.Instance.currentState != GameManager.GAME_STATE.IN_GAME)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (currentSelectWares == null)
            {
                if (IsHitWares())
                {
                    focusInZ = currentSelectWares.transform.position.z + 2.0f;
                    currentSelectWares.SelectWares();
                    SetCurrentWaresPosByMouse();
                }

            }
        }

        if (Input.GetMouseButton(0))
        {
            if (currentSelectWares != null)
            {
                SetCurrentWaresPosByMouse();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (currentSelectWares != null)
            {

                if (FindHittedShelf() != null)
                {
                    //remove tutorial in level 1
                    if (GameManager.Instance.currentLevel == 1)
                    {
                        GameManager.Instance.uiManager.gameView.HideTut();
                    }

                    ShelfController selectShelf = FindHittedShelf();

                    if (selectShelf.type == ShelfController.ShelfType.SINGLE)
                    {
                        currentSelectWares.UnSelectWares();
                        currentSelectWares.MoveBack();
                        currentSelectWares = null;
                    }

                    else
                    {
                        int insertIndex = FindHittedShelf().FindEmptySlotInShelf(currentSelectWares.transform.position);
                        if (insertIndex != -1)
                        {
                            AudioManager.instance.selectItemSound.Play();
                            ShelfController lastShelf = currentSelectWares.shelfController;
                            currentTargetShelf = FindHittedShelf();
                            currentSelectWares.MoveToAnotherShelf(currentTargetShelf, insertIndex);
                            lastShelf.CheckFirstRow();
                            currentSelectWares = null;
                            currentTargetShelf.CheckFirstRow();
                            currentTargetShelf = null;
                        }
                        else
                        {
                            currentSelectWares.UnSelectWares();
                            currentSelectWares.MoveBack();
                            currentSelectWares = null;
                        }
                    }


                }
                else
                {
                    currentSelectWares.UnSelectWares();
                    currentSelectWares.MoveBack();
                    currentSelectWares = null;
                }
            }
        }


    }

    // Tính toán vị trí chuột trong thế giới và di chuyển hàng hóa theo
    private void SetCurrentWaresPosByMouse()
    {
        mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(currentSelectWares.transform.position).z);
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 offsetAnchor = new Vector3(0.0f, 0.5f * GameManager.Instance.gamePlaySetting.tileSizeY, 0.0f);
        currentSelectWares.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, focusInZ) - offsetAnchor;
    }

    // Lấy danh sách ShelfController trong scene vừa load
    public void GetSelfContainerList()
    {
        Transform mainRoot = shelfRoot.GetChild(0);

        for (int i = 0; i < mainRoot.childCount; i++)
        {
            Transform childItem = mainRoot.GetChild(i);
            ShelfController shelfItem = childItem.GetComponent<ShelfController>();
            // Phân loại kệ để thêm vào danh sách tương ứng
            if (shelfItem.type == ShelfController.ShelfType.TRIPLE)
            {
                shelfItem.GenerateWarePoint();
                shelfControllerList.Add(shelfItem);
            }
            else if (shelfItem.type == ShelfController.ShelfType.SINGLE)
            {
                shelfItem.GenerateWarePoint();
                singlePushControllerList.Add(shelfItem);
            }

        }
    }

    // Sinh ra các đối tượng hàng hóa trên từng kệ dựa vào dữ liệu đã tính
    public void GenerateBoard()
    {
        // Bắt đầu tìm một món hàng ở hàng đầu tiên để làm gốc gợi ý
        for (int i = 0; i < shelfControllerList.Count; i++)
        {
            for (int j = 0; j < shelfControllerList[i].deptSize; j++)
            {
                for (int k = 0; k < shelfControllerList[i].colSize; k++)
                {
                    //int randomIndex = Random.RandomRange(-2, waresPrefabList.Count);
                    int randomIndex = GetProductIndex(cellListData[i].shelfTileList[j].tileList[k]);
                    //Debug.Log("PRODUCT ID " + cellListData[i].shelfTileList[j].tileList[k]);
                    if (randomIndex >= 0)
                    {
                        GameObject waresObject = Instantiate(waresPrefabList[randomIndex]);
                        waresObject.AddComponent<WaresController>();
                        waresObject.transform.position = shelfControllerList[i].shelfSlotList[j].warePointList[k].position;
                        waresObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                        waresObject.transform.SetParent(shelfControllerList[i].container.transform);
                        WaresController waresController = waresObject.GetComponent<WaresController>();
                        waresController.waresID = int.Parse(waresPrefabList[randomIndex].name);
                        waresController.InitWares();
                        waresController.shelfController = shelfControllerList[i];
                        waresController.colID = k;
                        waresController.deptID = j;
                        if (waresController.deptID > 0)
                            waresController.SetColorHide();
                        else
                            waresController.SetColorShow();

                        if (waresController.deptID >= 2)
                            waresController.SetVisible(false);
                        else
                            waresController.SetVisible(true);

                        shelfControllerList[i].shelfSlotList[j].waresInRowSlot.Add(waresObject.GetComponent<WaresController>());
                    }
                    else
                    {
                        shelfControllerList[i].shelfSlotList[j].waresInRowSlot.Add(null);
                    }

                }
            }

            // Sau khi đặt hàng hoá, loại bỏ các hàng rỗng để tối ưu hiển thị
            shelfControllerList[i].CleanEmptyRow();
        }

        // Nếu level có kệ đẩy đơn, khởi tạo thêm hàng hoá cho phần này
        if (singlePushCount > 0)
        {
            for (int i = 0; i < singlePushControllerList.Count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int waresIndex = GetProductIndex(singlePushTileList[i * 4 + j]);

                    if (waresIndex >= 0)
                    {
                        GameObject waresObject = Instantiate(waresPrefabList[waresIndex]);
                        waresObject.AddComponent<WaresController>();
                        waresObject.transform.position = singlePushControllerList[i].shelfSlotList[j].warePointList[0].position;
                        waresObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                        waresObject.transform.SetParent(singlePushControllerList[i].container.transform);
                        WaresController waresController = waresObject.GetComponent<WaresController>();
                        waresController.waresID = int.Parse(waresPrefabList[waresIndex].name);
                        waresController.InitWares();

                        waresController.shelfController = singlePushControllerList[i];
                        waresController.colID = 0;
                        waresController.deptID = j;

                        if (waresController.deptID > 0)
                            waresController.SetColorHide();
                        else
                            waresController.SetColorShow();

                        if (waresController.deptID >= 1)
                            waresController.SetVisible(false);
                        else
                            waresController.SetVisible(true);

                        singlePushControllerList[i].shelfSlotList[j].waresInRowSlot.Add(waresObject.GetComponent<WaresController>());
                    }

                    else
                    {
                        singlePushControllerList[i].shelfSlotList[j].waresInRowSlot.Add(null);
                    }
                }
            }
        }
    }

    // Tìm và gợi ý ba món hàng có thể ghép thành bộ
    public void ProcessHint()
    {
        List<HintItem> waresRemove = new List<HintItem>();
        // Bước 1: duyệt từng kệ để lấy món hàng ở hàng đầu tiên

        // Tiếp tục quét toàn bộ kệ để tìm hai món còn lại cùng loại
        for (int i = 0; i < shelfControllerList.Count; i++)
        {
            for (int j = 0; j < shelfControllerList[i].shelfSlotList[0].warePointList.Count; j++)
            {
                if (shelfControllerList[i].shelfSlotList[0].waresInRowSlot[j] != null)
                {
                    Debug.Log("Wares In First Row" + shelfControllerList[i].shelfSlotList[0].waresInRowSlot[j].waresID);
                    HintItem item = new HintItem();
                    item.waresController = shelfControllerList[i].shelfSlotList[0].waresInRowSlot[j];
                    item.shelfIndex = i;
                    item.slotIndex = 0;
                    item.elementIndex = j;
                    waresRemove.Add(item);
                    break;
                }
            }

            if (waresRemove.Count > 0)
                break;
        }
        // Bước 2: tiếp tục tìm 2 món còn lại trùng loại ở mọi vị trí

        for (int i = 0; i < shelfControllerList.Count; i++)
        {

            for (int j = 0; j < shelfControllerList[i].shelfSlotList.Count; j++)
            {
                for (int k = 0; k < shelfControllerList[i].shelfSlotList[j].warePointList.Count; k++)
                {
                    if (shelfControllerList[i].shelfSlotList[j].waresInRowSlot[k] != null)
                    {
                        //Debug.Log("Wares In First Row" + shelfControllerList[i].shelfSlotList[j].waresInRowSlot[k].waresID);

                        if (waresRemove.Count == 1)
                        {
                            if (waresRemove[0].waresController != shelfControllerList[i].shelfSlotList[j].waresInRowSlot[k]
                                && waresRemove[0].waresController.waresID == shelfControllerList[i].shelfSlotList[j].waresInRowSlot[k].waresID)
                            {
                                HintItem item = new HintItem();
                                item.waresController = shelfControllerList[i].shelfSlotList[j].waresInRowSlot[k];
                                item.shelfIndex = i;
                                item.slotIndex = j;
                                item.elementIndex = k;
                                waresRemove.Add(item);

                            }

                        }

                        if (waresRemove.Count == 2)
                        {
                            if (waresRemove[0].waresController != shelfControllerList[i].shelfSlotList[j].waresInRowSlot[k]
                                && waresRemove[1].waresController != shelfControllerList[i].shelfSlotList[j].waresInRowSlot[k]
                                && waresRemove[1].waresController.waresID == shelfControllerList[i].shelfSlotList[j].waresInRowSlot[k].waresID)
                            {
                                HintItem item = new HintItem();
                                item.waresController = shelfControllerList[i].shelfSlotList[j].waresInRowSlot[k];
                                item.shelfIndex = i;
                                item.slotIndex = j;
                                item.elementIndex = k;
                                waresRemove.Add(item);
                            }

                        }

                        if (waresRemove.Count == 3)
                        {
                            break;
                        }
                    }
                }

                if (waresRemove.Count >= 3)
                    break;

            }


            if (waresRemove.Count >= 3)
                break;
        }

        // Đủ ba món cùng loại -> hiển thị hiệu ứng gợi ý và loại chúng khỏi kệ
        if (waresRemove.Count == 3)
        {
            Debug.Log("Find pair ");



            for (int i = 0; i < waresRemove.Count; i++)
            {
                int shelfIndexRemove = waresRemove[i].shelfIndex;
                int slotIndexRemove = waresRemove[i].slotIndex;
                int elementIndexRemove = waresRemove[i].elementIndex;
                shelfControllerList[shelfIndexRemove].shelfSlotList[slotIndexRemove].waresInRowSlot[elementIndexRemove].SetVisible(true);
                shelfControllerList[shelfIndexRemove].shelfSlotList[slotIndexRemove].waresInRowSlot[elementIndexRemove].SetColorShow();
                shelfControllerList[shelfIndexRemove].shelfSlotList[slotIndexRemove].waresInRowSlot[elementIndexRemove] = null;

                RemoveHintItems(waresRemove[i].waresController.gameObject, i);
                StartCoroutine(RemoveHintItemsIE(waresRemove[i].waresController.gameObject));
                StartCoroutine(ShowHintVfx());
            }

            for (int i = 0; i < waresRemove.Count; i++)
            {
                int shelfIndexRemove = waresRemove[i].shelfIndex;
                shelfControllerList[shelfIndexRemove].CleanEmptyRow();
            }

            AudioManager.instance.PlayComboSound();
            GameManager.Instance.uiManager.gameView.UpdateStageProgress();
        }
        else
        {
            Debug.Log("no pair " + waresRemove.Count);
            if (singlePushControllerList.Count > 0)
            {
                Debug.Log("find pair in single push shelf ");
            }
        }
    }

    // Trộn ngẫu nhiên vị trí các hàng hóa trên từng kệ
    public void ProcessShuffle()
    {
        for (int i = 0; i < shelfControllerList.Count; i++)
        {
            ShuffleShelf(i);
        }
    }

    // Hoán đổi thứ tự các slot trong một kệ cụ thể
    private void ShuffleShelf(int shelfIndex)
    {
        List<ShelfSlot> slotRandomList = new List<ShelfSlot>();

        for (int i = 0; i < shelfControllerList[shelfIndex].shelfSlotList.Count; i++)
        {
            int randomIndex = Random.Range(0, shelfControllerList[shelfIndex].shelfSlotList.Count);
            ShelfSlot slot = shelfControllerList[shelfIndex].shelfSlotList[randomIndex];
            shelfControllerList[shelfIndex].shelfSlotList.RemoveAt(randomIndex);
            slotRandomList.Add(slot);
        }

        for (int i = 0; i < slotRandomList.Count; i++)
        {
            shelfControllerList[shelfIndex].shelfSlotList.Add(slotRandomList[i]);
        }
        shelfControllerList[shelfIndex].RePosition();
    }


    // Di chuyển item gợi ý lên khu vực hiển thị và hủy nó sau khi hoàn thành
    private void RemoveHintItems(GameObject hintItem, int itemIndex)
    {
        hintItem.transform.DOMove(new Vector3((itemIndex - 1) * GameManager.Instance.gamePlaySetting.tileSizeX, 9, 35), 0.25f).SetEase(Ease.InQuad).OnComplete
            (
            () =>
            {
                StartCoroutine(RemoveHintItemsIE(hintItem));
            }
            );

    }

    // Hủy đối tượng gợi ý sau một khoảng delay
    IEnumerator RemoveHintItemsIE(GameObject hintItem)
    {
        yield return new WaitForSeconds(1.0f);

        Destroy(hintItem);

    }

    // Hiệu ứng khi hiển thị gợi ý cho người chơi
    IEnumerator ShowHintVfx()
    {
        yield return new WaitForSeconds(1.0f);
        GameManager.Instance.comboVfx.transform.position = new Vector3(0, 10, 35);
        GameManager.Instance.comboVfx.Play();
        GameManager.Instance.clearVfx.transform.localPosition = new Vector3(0, 10, 35);
        GameManager.Instance.clearVfx.PlayAnim();
    }

    // Trả về vị trí của sản phẩm trong danh sách productIDList
    public int GetProductIndex(int productPara)
    {
        int productIndex = -1;

        for (int i = 0; i < productIDList.Length; i++)
        {
            if (productPara == productIDList[i])
            {
                productIndex = i;
                break;
            }
        }

        return productIndex;
    }

    // Raycast xác định kệ đang được trỏ tới bằng chuột
    public ShelfController FindHittedShelf()
    {
        ShelfController shelf = null;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f, shelfLayerMask))
        {
            ///Debug.Log("You selected the " + hit.transform.name);
            shelf = hit.transform.GetComponent<ShelfController>();
        }

        return shelf;

    }

    // Kiểm tra xem người chơi có đang nhấn vào một hàng hóa ở hàng đầu tiên hay không
    public bool IsHitWares()
    {
        bool isHit = false;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f, waresLayerMask))
        {
            if (hit.transform.GetComponent<WaresController>().deptID == 0 && hit.transform.GetComponent<WaresController>().currentState == WaresController.STATE.IDLE)
            {
                ///Debug.Log("You selected the " + hit.transform.name);
                isHit = true;
                currentSelectWares = hit.transform.GetComponent<WaresController>();
            }

        }

        return isHit;
    }

    #region GEN_MAP_FUNCTION
    // Tạo dữ liệu sắp xếp hàng hóa cho level: khởi tạo pool, xáo trộn và chia về từng kệ
    private void GenMap()
    {
        InitPairItems();

        LoadItemsToPool();

        LoadTiles();

        if (currentLv.ID > 1)
        {
            ShuffleElementInTiles();

            MixFirstColumn();

            MixRemainColumn();
        }



        FillData();
    }

    // Tính toán số cặp sản phẩm mỗi loại sẽ xuất hiện trên bàn
    private void InitPairItems()
    {
        productPairCountList = new int[productIDList.Length];

        for (int i = 0; i < productPairCountList.Length; i++)
        {
            productPairCountList[i] = 0;
        }

        int countForLoop = productPairCount;

        // Phân phối số lượng xuất hiện cho từng loại hàng hóa.
        // Vòng lặp chia đều dần dần để tránh chênh lệch quá lớn giữa các loại.
        while (countForLoop > 0)
        {
            for (int i = 0; i < productIDList.Length; i++)
            {
                productPairCountList[i]++;
                countForLoop--;
                if (countForLoop == 0)
                    break;
            }
        }

        /*
        if(3 * shelfCount > productPairCount)
        {
            positivePair = 3 * shelfCount - productPairCount;
            shelfDeepMax = 3;
        }
        else
        {
            positivePair = 4 * shelfCount - productPairCount;
            shelfDeepMax = 4;
        }
        */

        positivePair = shelfDeepMax * shelfCount - productPairCount;

        Debug.Log("Empty Shelf " + positivePair);

    }

    // Khởi tạo các pool chứa ID sản phẩm dựa trên số lượng cặp đã tính
    private void LoadItemsToPool()
    {
        // Khởi tạo các pool chứa ID sản phẩm. Mỗi pool tương ứng một loại hàng
        // Riêng pool cuối cùng (ID = -1) dùng để sinh ô trống trên kệ
        for (int i = 0; i <= productIDList.Length; i++)
        {
            ProductPool pPool = new ProductPool();

            if (i < productIDList.Length)
            {
                pPool.productID = productIDList[i];

                // Nhân 3 vì mỗi cặp sản phẩm sẽ chiếm 3 vị trí
                for (int j = 0; j < 3 * productPairCountList[i]; j++)
                {
                    pPool.productList.Add(pPool.productID);
                }
            }

            else
            {
                pPool.productID = -1;

                // Số lượng phần tử rỗng tương ứng với số ô trống cần có
                for (int j = 0; j < 3 * positivePair; j++)
                {
                    pPool.productList.Add(pPool.productID);
                }

                //if shelf has single push
                if (singlePushCount > 0)
                {
                    // Bổ sung ô trống cho các kệ đẩy đơn phía ngoài
                    for (int j = 0; j < 4 * singlePushCount; j++)
                    {
                        pPool.productList.Add(pPool.productID);
                    }
                }
            }

            productPoolList.Add(pPool);

        }

        // for (int i = 0; i < productPoolList.Count; i++)
        //     Debug.Log("PAIR " + productPoolList[i].productList.Count + "  =  " + productPoolList[i].productID);
    }

    // Sinh dữ liệu ô chứa cho toàn bộ kệ dựa trên pool sản phẩm đã chuẩn bị
    private void LoadTiles()
    {
        // Bước đầu tiên: tạo danh sách tile rỗng cho toàn bộ kệ
        // Mỗi tile tương ứng với 3 vị trí đặt hàng hóa trên kệ

        for (int i = 0; i < shelfDeepMax * shelfCount; i++)
        {
            ShelfTile tile = new ShelfTile();
            tile.tileList.Add(0);
            tile.tileList.Add(0);
            tile.tileList.Add(0);
            originalTileList.Add(tile);
        }

        // Trường hợp màn đầu tiên: thiết lập ô trống cố định cho hướng dẫn
        Debug.Log("Empty slot in first round : " + currentLv.roundsEmptyPlaceCount);

        // Nếu đây là màn đầu tiên (tutorial) thì bố trí một số ô trống cố định

        if (currentLv.ID == 1)
        {

            productPoolList[productPoolList.Count - 1].productList.RemoveAt(0);
            originalTileList[0].tileList[1] = -1;

            productPoolList[productPoolList.Count - 1].productList.RemoveAt(0);
            originalTileList[1].tileList[2] = -1;

            productPoolList[productPoolList.Count - 1].productList.RemoveAt(0);
            originalTileList[2].tileList[2] = -1;

            for (int i = shelfCount; i < shelfDeepMax * shelfCount; i++)
            {
            // Các kệ phía sau đều để trống nhằm tập trung hướng dẫn người chơi
                if (!productPoolList[productPoolList.Count - 1].isEmpty)
                {
                    productPoolList[productPoolList.Count - 1].productList.RemoveAt(0);
                    originalTileList[i].tileList[0] = -1;
                    originalTileList[i].tileList[1] = -1;
                    originalTileList[i].tileList[2] = -1;
                }

            }
            productPoolList.RemoveAt(productPoolList.Count - 1);

            Debug.Log("POOL SIZE " + productPoolList.Count);

            originalTileList[0].tileList[0] = productPoolList[1].productList[0];
            productPoolList[1].productList.RemoveAt(0);

            originalTileList[0].tileList[2] = productPoolList[0].productList[0];
            productPoolList[0].productList.RemoveAt(0);


            originalTileList[2].tileList[0] = productPoolList[1].productList[0];
            productPoolList[1].productList.RemoveAt(0);

            originalTileList[2].tileList[1] = productPoolList[1].productList[0];
            productPoolList[1].productList.RemoveAt(0);


            originalTileList[1].tileList[0] = productPoolList[0].productList[0];
            productPoolList[0].productList.RemoveAt(0);

            originalTileList[1].tileList[1] = productPoolList[0].productList[0];
            productPoolList[0].productList.RemoveAt(0);


            return;
        }

        // Đánh dấu ô trống ở cột đầu theo cấu hình level
        //Init first Column - 1
        for (int i = 0; i < shelfCount; i++)
        {
            if (i < currentLv.roundsEmptyPlaceCount)
            {
                // Debug.Log("POOL SIZE " + productPoolList[productPoolList.Count - 1].productList.Count);
                productPoolList[productPoolList.Count - 1].productList.RemoveAt(0);
                originalTileList[i].tileList[0] = -1;
            }

        }

        // Đánh dấu slot đầu tiên của các cột còn lại
        //Init remain Column - 1 -> for first slot
        for (int i = shelfCount; i < shelfDeepMax * shelfCount; i++)
        {
            if (!productPoolList[productPoolList.Count - 1].isEmpty)
            {
                productPoolList[productPoolList.Count - 1].productList.RemoveAt(0);
                originalTileList[i].tileList[0] = -1;
            }


        }
        //if remain -1 insert next slot 1 -> for second slot
        // Nếu vẫn còn ô trống, tiếp tục gán cho slot thứ hai
        if (productPoolList[productPoolList.Count - 1].productList.Count > 0)
        {
            for (int i = shelfCount; i < shelfDeepMax * shelfCount; i++)
            {
                if (!productPoolList[productPoolList.Count - 1].isEmpty)
                {
                    productPoolList[productPoolList.Count - 1].productList.RemoveAt(0);
                    originalTileList[i].tileList[1] = -1;
                }


            }
        }

        //if remain -1 for last slot
        // Nếu kệ sâu hơn 3 tầng và còn slot, gán cho ô cuối
        if (shelfDeepMax > 3 && productPoolList[productPoolList.Count - 1].productList.Count > 0)
        {
            if (productPoolList[productPoolList.Count - 1].productList.Count > 0)
            {
                for (int i = shelfCount; i < shelfDeepMax * shelfCount; i++)
                {
                    if (!productPoolList[productPoolList.Count - 1].isEmpty)
                    {
                        productPoolList[productPoolList.Count - 1].productList.RemoveAt(0);
                        originalTileList[i].tileList[2] = -1;
                    }


                }
            }
        }
        Debug.Log("POOL " + productPoolList[productPoolList.Count - 1].productList.Count);

        productPoolList.RemoveAt(productPoolList.Count - 1);


        //load data for single push tile

        if (singlePushCount > 0)
        {
            int rollIndex1 = 0;

            // Phân bổ hàng hóa cho phần kệ đẩy đơn
            for (int i = 0; i < 4 * singlePushCount; i++)
            {
                if (!productPoolList[rollIndex1].isEmpty)
                {
                    singlePushTileList.Add(productPoolList[rollIndex1].productList[0]);
                    productPoolList[rollIndex1].productList.RemoveAt(0);

                    if (productPoolList[rollIndex1].productList.Count == 0)
                    {
                        productPoolList.RemoveAt(rollIndex1);
                    }

                    // Di chuyển sang pool kế tiếp, vòng lại đầu danh sách nếu cần
                    if (rollIndex1 >= (productPoolList.Count - 1))
                    {
                        rollIndex1 = 0;
                    }
                    else
                    {
                        rollIndex1++;
                    }
                }
            }

            MixSinglePush();
        }
        // Gán ID sản phẩm cho các vị trí còn lại bằng cách rút từ pool

        int rollIndex = 0;
        // Điền nốt các vị trí còn lại trên kệ bằng cách rút tuần tự từ các pool
        for (int i = 0; i < shelfDeepMax * shelfCount; i++)
        {

            for (int j = 0; j < 3; j++)
            {
                if (originalTileList[i].tileList[j] != -1)
                {
                    if (!productPoolList[rollIndex].isEmpty)
                    {

                        originalTileList[i].tileList[j] = productPoolList[rollIndex].productList[0];
                        productPoolList[rollIndex].productList.RemoveAt(0);

                        if (productPoolList[rollIndex].productList.Count == 0)
                        {
                            productPoolList.RemoveAt(rollIndex);
                        }

                        // Chuyển sang pool tiếp theo, quay lại đầu khi hết danh sách
                        if (rollIndex >= (productPoolList.Count - 1))
                        {
                            rollIndex = 0;
                        }
                        else
                        {
                            rollIndex++;
                        }
                    }
                    else
                    {
                        productPoolList.RemoveAt(rollIndex);
                        rollIndex = FindIndexInPool();

                        originalTileList[i].tileList[j] = productPoolList[rollIndex].productList[0];
                        productPoolList[rollIndex].productList.RemoveAt(0);

                        if (productPoolList[rollIndex].productList.Count == 0)
                        {
                            productPoolList.RemoveAt(rollIndex);
                        }
                    }

                }
            }

        }

        //Debug.Log("POSITIVE POOL " + productPoolList.Count);

    }

    // Tìm index đầu tiên trong pool còn sản phẩm
    public int FindIndexInPool()
    {
        // Trả về index của pool còn sản phẩm gần nhất
        int index = -1;

        for (int i = 0; i < productPoolList.Count; i++)
        {
            if (productPoolList[i].productList.Count > 0)
            {
                index = i;
                return index;
            }
            else
            {
                productPoolList.RemoveAt(i);
            }
        }

        return index;
    }

    // Xáo trộn vị trí ba ô trong mỗi tile để tạo sự đa dạng, tránh việc sắp xếp theo mẫu cố định
    public void ShuffleElementInTiles()
    {
        for (int i = 0; i < originalTileList.Count; i++)
        {
            ShuffleTile(originalTileList[i]);
        }
    }

    // Hoán đổi ngẫu nhiên vị trí các phần tử trong một tile
    public void ShuffleTile(ShelfTile tile)
    {
        // Hoán đổi ngẫu nhiên vị trí các phần tử trong một tile
        // Giúp mỗi lần sinh map không trùng lặp hoàn toàn
        //Debug.Log("SHUFFLE");
        int randomIndex = Random.Range(0, 3);
        int temp;

        if (randomIndex == 0)
        {
            //SwapNumber(tile.tileList[0], tile.tileList[1]);
            temp = tile.tileList[0];
            tile.tileList[0] = tile.tileList[1];
            tile.tileList[1] = temp;
        }
        else if (randomIndex == 1)
        {
            //SwapNumber(tile.tileList[1], tile.tileList[2]);
            temp = tile.tileList[1];
            tile.tileList[1] = tile.tileList[2];
            tile.tileList[2] = temp;
        }
        else if (randomIndex == 2)
        {
            // SwapNumber(tile.tileList[0], tile.tileList[2]);
            temp = tile.tileList[0];
            tile.tileList[0] = tile.tileList[2];
            tile.tileList[2] = temp;
        }

    }

    // Xáo trộn danh sách sản phẩm cho các kệ đẩy đơn
    public void MixSinglePush()
    {
        // Xáo trộn thứ tự các sản phẩm dùng cho kệ đẩy đơn để tăng tính ngẫu nhiên
        List<int> tempList = new List<int>();

        for (int i = 0; i < 4 * singlePushCount; i++)
        {
            tempList.Add(singlePushTileList[i]);
        }

        for (int i = 0; i < 4 * singlePushCount; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            singlePushTileList.RemoveAt(singlePushTileList.Count - 1);
            singlePushTileList.Insert(0, tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
        }
    }

    // Trộn thứ tự cột đầu tiên để tránh trùng lặp dễ đoán
    public void MixFirstColumn()
    {
        // Xáo trộn thứ tự các tile ở cột đầu tiên để tạo cảm giác khác biệt giữa các ván chơi
        List<ShelfTile> tempList = new List<ShelfTile>();

        for (int i = 0; i < shelfCount; i++)
        {
            tempList.Add(originalTileList[i]);
        }

        for (int i = 0; i < shelfCount; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            originalTileList.RemoveAt(shelfCount - 1);
            originalTileList.Insert(0, tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
        }
    }

    // Trộn các cột còn lại
    public void MixRemainColumn()
    {
        // Tương tự MixFirstColumn nhưng áp dụng cho các cột còn lại
        List<ShelfTile> tempList = new List<ShelfTile>();

        for (int i = shelfCount; i < shelfDeepMax * shelfCount; i++)
        {
            tempList.Add(originalTileList[i]);
        }

        for (int i = shelfCount; i < shelfDeepMax * shelfCount; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            originalTileList.RemoveAt(shelfDeepMax * shelfCount - 1);
            originalTileList.Insert(shelfCount, tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
        }
    }


    // Chuyển dữ liệu tile gốc thành dạng ma trận dễ truy cập khi sinh kệ
    public void FillData()
    {
        // Chuyển danh sách tile gốc về cấu trúc ma trận theo từng kệ để tiện truy xuất
        cellListData = new List<ShelfCell>();

        for (int i = 0; i < shelfCount; i++)
        {
            ShelfCell shelfCell = new ShelfCell();

            for (int j = 0; j < shelfDeepMax; j++)
            {
                ShelfTile tile = new ShelfTile();
                for (int k = 0; k < 3; k++)
                {

                    tile.tileList.Add(originalTileList[j * shelfCount + i].tileList[k]);

                }
                shelfCell.shelfTileList.Add(tile);
            }

            cellListData.Add(shelfCell);
        }


    }
    #endregion
}

#region CLASS_GEN_MAP
[System.Serializable]

// Lưu trữ tạm các ID sản phẩm dùng để phân phát cho tile
public class ProductPool
{
    public int productID;

    public List<int> productList = new List<int>();

    public bool isEmpty => productList.Count == 0;
}

[System.Serializable]

// Đại diện cho 1 ô trên kệ chứa tối đa 3 vị trí sản phẩm
public class ShelfTile
{
    public List<int> tileList = new List<int>();
}

[System.Serializable]

// Tập hợp nhiều ShelfTile để mô tả một kệ hoàn chỉnh
public class ShelfCell
{
    public List<ShelfTile> shelfTileList = new List<ShelfTile>();
}

[System.Serializable]

// Thông tin dùng cho hệ thống gợi ý tìm bộ sản phẩm
public class HintItem
{
    public WaresController waresController;

    public int shelfIndex;

    public int slotIndex;

    public int elementIndex;
}

#endregion