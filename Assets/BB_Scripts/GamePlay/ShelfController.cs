using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Lofelt.NiceVibrations;

/// <summary>
/// Quản lý kệ hàng và các hàng chứa vật phẩm trong màn chơi.
/// Phụ trách sinh điểm đặt vật phẩm, kiểm tra hàng đầu tiên
/// và xử lý khi đủ bộ ba giống nhau.
/// </summary>
public class ShelfController : MonoBehaviour
{

    // Số cột tối đa của kệ
    [HideInInspector] public int colSize;

    // Độ sâu của kệ (số hàng đặt vật phẩm)
    [HideInInspector] public int deptSize;

    // Tham chiếu tới container chứa các điểm đặt vật phẩm
    [HideInInspector] public ShelfContainer container;

    // Danh sách các hàng (slot) trên kệ
    public List<ShelfSlot> shelfSlotList = new List<ShelfSlot>();

    // Kiểu cấu trúc của kệ
    public enum ShelfType
    {
        TRIPLE, // ba cột
        SINGLE  // một cột
    }

    // Kiểu kệ: ba cột hoặc một cột
    public ShelfType type;

    // Đánh dấu kệ đã xử lý xong hết hàng hay chưa
    public bool isFinish;

    // Vị trí gốc của từng cột dùng để tính toán sắp xếp vật phẩm
    private Vector3 waresRoot0;
    private Vector3 waresRoot1;
    private Vector3 waresRoot2;

    // Start is called before the first frame update
    // Hàm khởi tạo, reset trạng thái kệ khi bắt đầu màn chơi
    void Start()
    {
        isFinish = false;
    }

    // Update is called once per frame
    // Không sử dụng trong phiên bản hiện tại nhưng giữ để mở rộng về sau
    void Update()
    {

    }

    // Sinh ra các vị trí đặt vật phẩm cho từng hàng của kệ
    public void GenerateWarePoint()
    {

        container = transform.GetChild(0).GetComponent<ShelfContainer>();

        colSize = GameManager.Instance.gamePlaySetting.colSizeMax;
        //deptSize = GameManager.Instance.gamePlaySetting.deptSizeMax;
        deptSize = GameManager.Instance.gameBoard.shelfDeepMax;

        if (type == ShelfType.TRIPLE)
        {
            for (int i = 0; i < deptSize; i++)
            {
                ShelfSlot shelfSlost = new ShelfSlot();

                for (int j = 0; j < colSize; j++)
                {
                    GameObject point = new GameObject();
                    point.transform.parent = container.transform;
                    point.transform.localPosition = Vector3.zero;
                    point.transform.localPosition = new Vector3((2 * j + 1) * 0.5f * GameManager.Instance.gamePlaySetting.tileSizeX, 0.0f, -i * GameManager.Instance.gamePlaySetting.tileSizeZ);
                    point.name = "ware " + i.ToString() + "_" + j.ToString();

                    shelfSlost.warePointList.Add(point.transform);

                }

                shelfSlotList.Add(shelfSlost);
            }

            waresRoot0 = shelfSlotList[0].warePointList[0].localPosition;
            waresRoot1 = shelfSlotList[0].warePointList[1].localPosition;
            waresRoot2 = shelfSlotList[0].warePointList[2].localPosition;
        }

        else if (type == ShelfType.SINGLE)
        {

            for (int i = 0; i < 4; i++)
            {
                ShelfSlot shelfSlost = new ShelfSlot();

                GameObject point = new GameObject();
                point.transform.parent = container.transform;
                point.transform.localPosition = Vector3.zero;

                point.transform.localPosition = new Vector3(0.5f * GameManager.Instance.gamePlaySetting.tileSizeX,
                    0.0f, -i * GameManager.Instance.gamePlaySetting.tileSizeZ);
                point.name = "ware " + i.ToString();

                shelfSlost.warePointList.Add(point.transform);

                shelfSlotList.Add(shelfSlost);
            }

        }



    }

    // Tạo hàng rỗng đầu tiên khi kệ không còn hàng nào
    private void GenEmptyFirstRow()
    {
        container = transform.GetChild(0).GetComponent<ShelfContainer>();

        colSize = GameManager.Instance.gamePlaySetting.colSizeMax;
        //deptSize = GameManager.Instance.gamePlaySetting.deptSizeMax;
        deptSize = GameManager.Instance.gameBoard.shelfDeepMax;

        if (type == ShelfType.TRIPLE)
        {
            ShelfSlot shelfSlost = new ShelfSlot();

            for (int j = 0; j < colSize; j++)
            {
                GameObject point = new GameObject();
                point.transform.parent = container.transform;
                point.transform.localPosition = Vector3.zero;
                point.transform.localPosition = new Vector3((2 * j + 1) * 0.5f * GameManager.Instance.gamePlaySetting.tileSizeX, 0.0f, 0.0f);
                point.name = "ware0 " + "_" + j.ToString();
                shelfSlost.warePointList.Add(point.transform);
                shelfSlost.waresInRowSlot.Add(null);
            }

            shelfSlotList.Add(shelfSlost);

        }
    }

    // Loại bỏ các hàng không chứa vật phẩm và căn chỉnh lại vị trí các hàng còn lại
    public void CleanEmptyRow()
    {
        bool checkEmpty = false;
        List<ShelfSlot> emptySlotList = new List<ShelfSlot>();

        for (int i = 0; i < shelfSlotList.Count; i++)
        {
            if (shelfSlotList[i].waresInRowSlot[0] == null && shelfSlotList[i].waresInRowSlot[1] == null && shelfSlotList[i].waresInRowSlot[2] == null)
            {
                //shelfSlotList.RemoveAt(i);
                //Debug.Log("EMPTY ROW");
                emptySlotList.Add(shelfSlotList[i]);
                checkEmpty = true;
            }
        }

        // if (!checkEmpty)
        //   return;

        //Vector3 waresRoot0 = shelfSlotList[0].warePointList[0].localPosition;
        //Vector3 waresRoot1 = shelfSlotList[0].warePointList[1].localPosition;
        //Vector3 waresRoot2 = shelfSlotList[0].warePointList[2].localPosition;


        for (int i = 0; i < emptySlotList.Count; i++)
        {
            shelfSlotList.Remove(emptySlotList[i]);
        }


        for (int i = 0; i < shelfSlotList.Count; i++)
        {


            shelfSlotList[i].warePointList[0].localPosition = waresRoot0 - new Vector3(0.0f, 0.0f, GameManager.Instance.gamePlaySetting.tileSizeZ) * i;
            shelfSlotList[i].warePointList[1].localPosition = waresRoot1 - new Vector3(0.0f, 0.0f, GameManager.Instance.gamePlaySetting.tileSizeZ) * i;
            shelfSlotList[i].warePointList[2].localPosition = waresRoot2 - new Vector3(0.0f, 0.0f, GameManager.Instance.gamePlaySetting.tileSizeZ) * i;

            if (shelfSlotList[i].waresInRowSlot[0] != null)
            {
                shelfSlotList[i].waresInRowSlot[0].transform.localPosition = shelfSlotList[i].warePointList[0].localPosition;
                shelfSlotList[i].waresInRowSlot[0].deptID = i;

                if (shelfSlotList[i].waresInRowSlot[0].deptID > 0)
                    shelfSlotList[i].waresInRowSlot[0].SetColorHide();
                else
                    shelfSlotList[i].waresInRowSlot[0].SetColorShow();

                if (shelfSlotList[i].waresInRowSlot[0].deptID >= 2)
                    shelfSlotList[i].waresInRowSlot[0].SetVisible(false);
                else
                    shelfSlotList[i].waresInRowSlot[0].SetVisible(true);

            }

            if (shelfSlotList[i].waresInRowSlot[1] != null)
            {
                shelfSlotList[i].waresInRowSlot[1].transform.localPosition = shelfSlotList[i].warePointList[1].localPosition;
                shelfSlotList[i].waresInRowSlot[1].deptID = i;

                if (shelfSlotList[i].waresInRowSlot[1].deptID > 0)
                    shelfSlotList[i].waresInRowSlot[1].SetColorHide();
                else
                    shelfSlotList[i].waresInRowSlot[1].SetColorShow();

                if (shelfSlotList[i].waresInRowSlot[1].deptID >= 2)
                    shelfSlotList[i].waresInRowSlot[1].SetVisible(false);
                else
                    shelfSlotList[i].waresInRowSlot[1].SetVisible(true);
            }

            if (shelfSlotList[i].waresInRowSlot[2] != null)
            {
                shelfSlotList[i].waresInRowSlot[2].transform.localPosition = shelfSlotList[i].warePointList[2].localPosition;
                shelfSlotList[i].waresInRowSlot[2].deptID = i;

                if (shelfSlotList[i].waresInRowSlot[2].deptID > 0)
                    shelfSlotList[i].waresInRowSlot[2].SetColorHide();
                else
                    shelfSlotList[i].waresInRowSlot[2].SetColorShow();

                if (shelfSlotList[i].waresInRowSlot[2].deptID >= 2)
                    shelfSlotList[i].waresInRowSlot[2].SetVisible(false);
                else
                    shelfSlotList[i].waresInRowSlot[2].SetVisible(true);
            }



        }

        if (shelfSlotList.Count == 0)
            GenEmptyFirstRow();
    }

    // Cập nhật lại vị trí của toàn bộ hàng dựa trên thông tin gốc
    public void RePosition()
    {
        for (int i = 0; i < shelfSlotList.Count; i++)
        {


            shelfSlotList[i].warePointList[0].localPosition = waresRoot0 - new Vector3(0.0f, 0.0f, GameManager.Instance.gamePlaySetting.tileSizeZ) * i;
            shelfSlotList[i].warePointList[1].localPosition = waresRoot1 - new Vector3(0.0f, 0.0f, GameManager.Instance.gamePlaySetting.tileSizeZ) * i;
            shelfSlotList[i].warePointList[2].localPosition = waresRoot2 - new Vector3(0.0f, 0.0f, GameManager.Instance.gamePlaySetting.tileSizeZ) * i;

            if (shelfSlotList[i].waresInRowSlot[0] != null)
            {
                shelfSlotList[i].waresInRowSlot[0].transform.localPosition = shelfSlotList[i].warePointList[0].localPosition;
                shelfSlotList[i].waresInRowSlot[0].deptID = i;

                if (shelfSlotList[i].waresInRowSlot[0].deptID > 0)
                    shelfSlotList[i].waresInRowSlot[0].SetColorHide();
                else
                    shelfSlotList[i].waresInRowSlot[0].SetColorShow();

                if (shelfSlotList[i].waresInRowSlot[0].deptID >= 2)
                    shelfSlotList[i].waresInRowSlot[0].SetVisible(false);
                else
                    shelfSlotList[i].waresInRowSlot[0].SetVisible(true);

            }

            if (shelfSlotList[i].waresInRowSlot[1] != null)
            {
                shelfSlotList[i].waresInRowSlot[1].transform.localPosition = shelfSlotList[i].warePointList[1].localPosition;
                shelfSlotList[i].waresInRowSlot[1].deptID = i;

                if (shelfSlotList[i].waresInRowSlot[1].deptID > 0)
                    shelfSlotList[i].waresInRowSlot[1].SetColorHide();
                else
                    shelfSlotList[i].waresInRowSlot[1].SetColorShow();

                if (shelfSlotList[i].waresInRowSlot[1].deptID >= 2)
                    shelfSlotList[i].waresInRowSlot[1].SetVisible(false);
                else
                    shelfSlotList[i].waresInRowSlot[1].SetVisible(true);
            }

            if (shelfSlotList[i].waresInRowSlot[2] != null)
            {
                shelfSlotList[i].waresInRowSlot[2].transform.localPosition = shelfSlotList[i].warePointList[2].localPosition;
                shelfSlotList[i].waresInRowSlot[2].deptID = i;

                if (shelfSlotList[i].waresInRowSlot[2].deptID > 0)
                    shelfSlotList[i].waresInRowSlot[2].SetColorHide();
                else
                    shelfSlotList[i].waresInRowSlot[2].SetColorShow();

                if (shelfSlotList[i].waresInRowSlot[2].deptID >= 2)
                    shelfSlotList[i].waresInRowSlot[2].SetVisible(false);
                else
                    shelfSlotList[i].waresInRowSlot[2].SetVisible(true);
            }



        }
    }

    // Tìm vị trí trống gần nhất với vị trí chọn hiện tại trên hàng đầu tiên
    public int FindEmptySlotInShelf(Vector3 selectedWaresPos)
    {
        int emptySlotIndex = -1;
        float lastDistance = 0.0f;
        int numberofIndex = 0;

        for (int i = 0; i < shelfSlotList[0].waresInRowSlot.Count; i++)
        {
            if (shelfSlotList[0].waresInRowSlot[i] == null)
            {
                if (numberofIndex > 0)
                {
                    float newDistance = Vector3.Distance(selectedWaresPos, shelfSlotList[0].warePointList[i].position);
                    if (newDistance < lastDistance)
                    {
                        emptySlotIndex = i;
                        lastDistance = newDistance;
                    }
                }
                else
                {
                    emptySlotIndex = i;
                    lastDistance = Vector3.Distance(selectedWaresPos, shelfSlotList[0].warePointList[i].position);
                }

                numberofIndex++;
            }

        }

        return emptySlotIndex;
    }

    // Kiểm tra hàng đầu tiên để xem có đủ bộ ba giống nhau hay không
    public void CheckFirstRow()
    {
        if (type == ShelfType.TRIPLE)
        {
            if (shelfSlotList[0].waresInRowSlot[0] == null && shelfSlotList[0].waresInRowSlot[1] == null && shelfSlotList[0].waresInRowSlot[2] == null)
                PushRow();

            if (shelfSlotList[0].waresInRowSlot[0] == null || shelfSlotList[0].waresInRowSlot[1] == null || shelfSlotList[0].waresInRowSlot[2] == null)
                return;

            if ((shelfSlotList[0].waresInRowSlot[0].waresID == shelfSlotList[0].waresInRowSlot[1].waresID) &&
                (shelfSlotList[0].waresInRowSlot[1].waresID == shelfSlotList[0].waresInRowSlot[2].waresID))
                ClearFirstRow();
        }
        else if (type == ShelfType.SINGLE)
        {
            if (shelfSlotList.Count > 0)
                PushSingleShelf();
        }

    }

    // Xóa ba vật phẩm ở hàng đầu tiên kèm hiệu ứng
    private void ClearFirstRow()
    {
        shelfSlotList[0].waresInRowSlot[0].currentState = WaresController.STATE.PROCESS;
        shelfSlotList[0].waresInRowSlot[1].currentState = WaresController.STATE.PROCESS;
        shelfSlotList[0].waresInRowSlot[2].currentState = WaresController.STATE.PROCESS;

        shelfSlotList[0].waresInRowSlot[0].transform.DOLocalMoveX(GameManager.Instance.gamePlaySetting.tileSizeX, 0.15f).SetRelative().SetEase(Ease.Linear).SetDelay(0.25f).OnComplete(() =>
        {
            shelfSlotList[0].waresInRowSlot[0].currentState = WaresController.STATE.IDLE;
            shelfSlotList[0].waresInRowSlot[0].RemoveItem();

        });

        shelfSlotList[0].waresInRowSlot[2].transform.DOLocalMoveX(-GameManager.Instance.gamePlaySetting.tileSizeX, 0.15f).SetRelative().SetEase(Ease.Linear).SetDelay(0.25f).OnComplete(() =>
        {

            shelfSlotList[0].waresInRowSlot[1].currentState = WaresController.STATE.IDLE;
            shelfSlotList[0].waresInRowSlot[2].currentState = WaresController.STATE.IDLE;
            shelfSlotList[0].waresInRowSlot[2].RemoveItem();
            shelfSlotList[0].waresInRowSlot[1].RemoveItem();
            PushRow();
            GameManager.Instance.comboVfx.transform.position = shelfSlotList[0].warePointList[1].position + new Vector3(0, GameManager.Instance.gamePlaySetting.tileSizeY * 0.5f, 1.0f);
            GameManager.Instance.comboVfx.Play();
            GameManager.Instance.clearVfx.transform.localPosition = shelfSlotList[0].warePointList[1].position + new Vector3(0, GameManager.Instance.gamePlaySetting.tileSizeY * 1.0f, 1.0f);
            GameManager.Instance.clearVfx.PlayAnim();
            Vector3 starSpawnPos = shelfSlotList[0].warePointList[1].position + new Vector3(0, GameManager.Instance.gamePlaySetting.tileSizeY * 0.5f, 1.0f);
            AudioManager.instance.PlayComboSound();

            if (AudioManager.instance.hapticState == 1)
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);

            GameManager.Instance.uiManager.gameView.UpdateStageProgress();
            if (GameManager.Instance.uiManager.gameView.comboCount >= 2)
                GameManager.Instance.starRoot.SpawnStar(starSpawnPos, GameManager.Instance.uiManager.gameView.comboCount);
        });


    }

    // Đẩy toàn bộ hàng về phía trước khi hàng đầu tiên bị xóa
    private void PushRow()
    {

        if (shelfSlotList.Count > 1)
        {
            for (int i = 0; i < shelfSlotList.Count; i++)
            {
                for (int j = 0; j < shelfSlotList[i].waresInRowSlot.Count; j++)
                {
                    if (shelfSlotList[i].waresInRowSlot[j] != null)
                    {
                        shelfSlotList[i].waresInRowSlot[j].currentState = WaresController.STATE.PROCESS;
                        shelfSlotList[i].waresInRowSlot[j].deptID--;

                        if (shelfSlotList[i].waresInRowSlot[j].deptID > 0)
                            shelfSlotList[i].waresInRowSlot[j].SetColorHide();
                        else
                            shelfSlotList[i].waresInRowSlot[j].SetColorShow();

                        if (shelfSlotList[i].waresInRowSlot[j].deptID >= 2)
                            shelfSlotList[i].waresInRowSlot[j].SetVisible(false);
                        else
                            shelfSlotList[i].waresInRowSlot[j].SetVisible(true);

                        shelfSlotList[i].waresInRowSlot[j].transform.DOLocalMoveZ(GameManager.Instance.gamePlaySetting.tileSizeZ, 0.15f).SetRelative().SetEase(Ease.Linear).OnComplete(() =>
                        {
                            //CheckFirstRow();

                        });
                    }

                    shelfSlotList[i].warePointList[j].localPosition += new Vector3(0.0f, 0.0f, GameManager.Instance.gamePlaySetting.tileSizeZ);

                }

                StartCoroutine(ResetStateIE());
            }
        }


        if (shelfSlotList.Count > 1)
            shelfSlotList.RemoveAt(0);

        if (shelfSlotList.Count == 1)
        {
            // Debug.Log("LAST ROW");
            isFinish = true;
        }

    }

    // Chờ một khoảng thời gian ngắn trước khi đặt lại trạng thái của kệ
    private IEnumerator ResetStateIE()
    {
        yield return new WaitForSeconds(0.2f);
        ResetState();
    }

    // Đặt lại trạng thái của tất cả vật phẩm về IDLE
    private void ResetState()
    {
        if (shelfSlotList.Count > 0)
        {
            for (int i = 0; i < shelfSlotList.Count; i++)
            {
                for (int j = 0; j < shelfSlotList[i].waresInRowSlot.Count; j++)
                {
                    if (shelfSlotList[i].waresInRowSlot[j] != null)
                        shelfSlotList[i].waresInRowSlot[j].currentState = WaresController.STATE.IDLE;
                }
            }
        }
    }

    // Đẩy vật phẩm trong kệ đơn (một cột) về phía trước
    private void PushSingleShelf()
    {
        if (shelfSlotList.Count > 0)
        {
            for (int j = 0; j < shelfSlotList.Count; j++)
            {
                if (shelfSlotList[j].waresInRowSlot[0] != null)
                {
                    shelfSlotList[j].waresInRowSlot[0].currentState = WaresController.STATE.PROCESS;
                    Debug.Log("SINGLE PROCESS");
                    shelfSlotList[j].waresInRowSlot[0].deptID--;

                    if (shelfSlotList[j].waresInRowSlot[0].deptID > 0)
                        shelfSlotList[j].waresInRowSlot[0].SetColorHide();
                    else
                        shelfSlotList[j].waresInRowSlot[0].SetColorShow();

                    if (shelfSlotList[j].waresInRowSlot[0].deptID >= 1)
                        shelfSlotList[j].waresInRowSlot[0].SetVisible(false);
                    else
                        shelfSlotList[j].waresInRowSlot[0].SetVisible(true);

                    shelfSlotList[j].waresInRowSlot[0].currentState = WaresController.STATE.IDLE;

                    shelfSlotList[j].waresInRowSlot[0].transform.DOLocalMoveZ(GameManager.Instance.gamePlaySetting.tileSizeZ, 0.15f).SetRelative().SetEase(Ease.Linear).OnComplete(() =>
                    {


                    });


                }
                shelfSlotList[j].warePointList[0].localPosition += new Vector3(0.0f, 0.0f, GameManager.Instance.gamePlaySetting.tileSizeZ);
                //StartCoroutine(ResetStateIE());
            }
        }

        if (shelfSlotList.Count > 0)
            shelfSlotList.RemoveAt(0);

        if (shelfSlotList.Count == 0)
        {
            // Debug.Log("LAST ROW");

        }
    }
}

[System.Serializable]
// Đại diện cho một hàng trong kệ, lưu trữ danh sách vật phẩm và vị trí của chúng
public class ShelfSlot
{
    // Danh sách vật phẩm hiện có trong hàng
    public List<WaresController> waresInRowSlot = new List<WaresController>();

    // Danh sách vị trí đặt vật phẩm tương ứng
    public List<Transform> warePointList = new List<Transform>();

}
