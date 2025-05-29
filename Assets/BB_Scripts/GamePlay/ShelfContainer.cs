using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lớp này chỉ dùng để đánh dấu GameObject con làm container của kệ.
// ShelfController sẽ dựa vào component này để tìm vị trí sinh điểm chứa hàng hoá.
// Không có logic thực thi nào tại đây nhưng tách thành script riêng
// giúp quản lý Hierarchy dễ dàng và thuận tiện mở rộng khi cần.

public class ShelfContainer : MonoBehaviour
{
    // Hàm Start và Update được giữ lại để dễ dàng bổ sung chức năng nếu cần.
    // Hiện tại container không có xử lý gì trong mỗi khung hình.
    void Start()
    {
        // Không cần khởi tạo đặc biệt.
    }

    void Update()
    {
        // Không có logic cập nhật.
    }
}
