using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;

public class LevelParser : MonoBehaviour
{
    /// <summary>
    /// File CSV chứa thông tin cấu hình từng màn chơi
    /// </summary>
    public TextAsset levelCsvFile;

    /// <summary>
    /// Lưu dữ liệu đã tách từ file CSV dưới dạng mảng 2 chiều
    /// </summary>
    string[][] dataArr = null;

    /// <summary>
    /// Danh sách dữ liệu màn chơi sau khi đã parse từ file
    /// </summary>
    public List<LevelDataModel> levelDataList;

    // Hàm khởi tạo, đọc dữ liệu ngay khi script được chạy
    void Start()
    {
        levelDataList = new List<LevelDataModel>();
        ParseData();
    }

    // Lắng nghe phím tắt trong quá trình phát triển
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            for(int i = 100; i < 200; i++)
                CreateLevelFile(i);
        }
           
    }

    /// <summary>
    /// Đọc dữ liệu từ file CSV và lưu vào danh sách levelDataList
    /// </summary>
    void ParseData()
    {
        // i = dòng, j = cột trong file CSV
        dataArr = CsvParser2.Parse(levelCsvFile.text);

        for (int i = 0; i < dataArr.Length; i++)
        {
            // Bỏ qua 4 dòng đầu tiên vì chứa tiêu đề trong file
            if (i >= 4)
            {
                LevelDataModel lv = new LevelDataModel();
                for (int j = 0; j < dataArr[i].Length; j++)
                {
                    // Mỗi cột tương ứng với một thuộc tính của LevelDataModel
                    if (j == 0)
                        lv.ID = int.Parse(dataArr[i][0]);
                    else if (j == 1)
                        lv.sceneID = int.Parse(dataArr[i][1]);
                    else if (j == 3)
                        lv.time = int.Parse(dataArr[i][3]);
                    else if (j == 6)
                        lv.totalProductCount = int.Parse(dataArr[i][6]);
                    else if (j == 8)
                    {
                        // Cột này có thể để trống nên cần kiểm tra
                        if(dataArr[i][8] != "")
                            lv.dynamicProductSet = int.Parse(dataArr[i][8]);
                    }
                        
                    else if (j == 12)
                        lv.singlePush= int.Parse(dataArr[i][12]);
                    else if (j == 13)
                    {
                        string[] prefix = dataArr[i][13].Split('|');
                        if (prefix[0] != "")
                            lv.roundsEmptyPlaceCount = int.Parse(prefix[0]);
                    }
                        
                    

                  
                }

                // Lưu model vừa parse vào danh sách
                levelDataList.Add(lv);
            }


        }
    }

    /// <summary>
    /// Tạo ScriptableObject LevelData tương ứng với chỉ số màn
    /// </summary>
    /// <param name="lvIndex">Chỉ số màn trong danh sách levelDataList</param>
    public void CreateLevelFile(int lvIndex)
    {
#if UNITY_EDITOR
        LevelData levelSO = ScriptableObject.CreateInstance<LevelData>();
        levelSO.ID = levelDataList[lvIndex].ID;
        levelSO.sceneID = levelDataList[lvIndex].sceneID;
        levelSO.time = levelDataList[lvIndex].time;
        levelSO.totalProductCount = levelDataList[lvIndex].totalProductCount;
        levelSO.dynamicProductSet = levelDataList[lvIndex].dynamicProductSet;
        levelSO.roundsEmptyPlaceCount = levelDataList[lvIndex].roundsEmptyPlaceCount;
        levelSO.singlePush = levelDataList[lvIndex].singlePush;
        // Lưu asset mới vào thư mục Resources/GameLevels
        UnityEditor.AssetDatabase.CreateAsset(levelSO, "Assets/Resources/GameLevels/" + "Level" + (lvIndex + 1).ToString() + ".asset");
#endif
    }
}

[System.Serializable]
public class LevelDataModel
{
    // ID của màn chơi
    public int ID;

    // ID của scene tương ứng trong thư mục Shelves
    public int sceneID;

    // Thời gian giới hạn của màn chơi (giây)
    public int time;

    // Tổng số sản phẩm cần sinh trong màn
    public int totalProductCount;

    // Bộ sản phẩm được sử dụng cho màn chơi
    public int dynamicProductSet;

    // Số lượt đẩy kệ đơn (single push)
    public int singlePush;

    // Số ô trống cần tạo sau mỗi vòng
    public int roundsEmptyPlaceCount;
}
