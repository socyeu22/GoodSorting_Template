# Tài liệu Onboarding Chi tiết cho Intern - Dự án Game Sắp Xếp Hàng Hóa

**Ngày:** 30 tháng 5 năm 2025

**Phiên bản:** 1.1 (Dành cho Intern)

**Mục lục:**

1.  [Chào mừng bạn đến với Dự án!](#1-chào-mừng-bạn-đến-với-dự-án)
    1.1. [Game của chúng ta là gì?](#11-game-của-chúng-ta-là-gì)
    1.2. [Chúng ta xây dựng game bằng gì? (Công cụ và Thư viện)](#12-chúng-ta-xây-dựng-game-bằng-gì-công-cụ-và-thư-viện)
2.  [Tìm hiểu "Bộ Não" của Game: Kiến trúc Hệ thống](#2-tìm-hiểu-bộ-não-của-game-kiến-trúc-hệ-thống)
    2.1. [Dữ liệu Game được lưu trữ và quản lý như thế nào?](#21-dữ-liệu-game-được-lưu-trữ-và-quản-lý-như-thế-nào)
    2.2. [Dựng Cảnh Chơi: Tạo ra bàn chơi (Board Generation)](#22-dựng-cảnh-chơi-tạo-ra-bàn-chơi-board-generation)
    2.3. [Cách Người Chơi Tương Tác: Gameplay Chính](#23-cách-người-chơi-tương-tác-gameplay-chính)
    2.4. [Ai là Số Một? Hệ thống Xếp hạng](#24-ai-là-số-một-hệ-thống-xếp-hạng)
    2.5. [Cho Game Thêm "Lấp Lánh": Hiệu ứng Hình ảnh (VFX) và Âm thanh](#25-cho-game-thêm-lấp-lánh-hiệu-ứng-hình-ảnh-vfx-và-âm-thanh)
    2.6. ["Người Duy Nhất": Lớp Singleton - Quản lý Toàn cục](#26-người-duy-nhất-lớp-singleton---quản-lý-toàn-cục)
3.  ["Hồ sơ Nhân sự" của các Script Chính: Tìm hiểu từng File Code](#3-hồ-sơ-nhân-sự-của-các-script-chính-tìm-hiểu-từng-file-code)
4.  [Dòng Chảy Công Việc và Dữ Liệu: Từ Ý Tưởng Đến Gameplay](#4-dòng-chảy-công-việc-và-dữ-liệu-từ-ý-tưởng-đến-gameplay)
    4.1. [Hành trình của Dữ liệu Màn chơi (Level)](#41-hành-trình-của-dữ-liệu-màn-chơi-level)
    4.2. [Hành trình của Dữ liệu Sản phẩm (Wares)](#42-hành-trình-của-dữ-liệu-sản-phẩm-wares)
    4.3. ["Cẩm nang" cho Công việc Hàng ngày của bạn](#43-cẩm-nang-cho-công-việc-hàng-ngày-của-bạn)
5.  ["Từ điển" Các Thuật Ngữ và Cấu Trúc Dữ Liệu Quan Trọng](#5-từ-điển-các-thuật-ngữ-và-cấu-trúc-dữ-liệu-quan-trọng)
6.  ["Đồng Minh" Từ Bên Ngoài: Các Thư Viện Sử Dụng](#6-đồng-minh-từ-bên-ngoài-các-thư-viện-sử-dụng)
7.  [Lời khuyên cho Thực tập sinh](#7-lời-khuyên-cho-thực-tập-sinh)

---

## 1. Chào mừng bạn đến với Dự án!

Chào mừng bạn đã gia nhập đội ngũ phát triển! Tài liệu này sẽ là người bạn đồng hành giúp bạn hiểu rõ hơn về dự án game chúng ta đang xây dựng, từ cấu trúc tổng thể đến chi tiết từng dòng code. Mục tiêu là để bạn có thể tự tin bắt tay vào công việc và đóng góp những ý tưởng tuyệt vời của mình.

### 1.1. Game của chúng ta là gì?

Chúng ta đang làm một game giải đố trên điện thoại di động. Hãy tưởng tượng bạn là một người quản lý siêu thị tài ba, nhiệm vụ của bạn là sắp xếp các món hàng hóa (gọi là "wares") trên những chiếc kệ ("shelves").

* **Cách chơi chính:** Người chơi sẽ chạm và kéo các món hàng từ hàng ngoài cùng của các kệ này.
* **Mục tiêu:** Sắp xếp sao cho có 3 món hàng giống hệt nhau nằm trên cùng một hàng của một chiếc kệ. Khi đó, chúng sẽ "biến mất" và bạn ghi điểm!
* **Thử thách:** Mỗi màn chơi (level) sẽ có những yêu cầu riêng, ví dụ như giới hạn thời gian, hoặc số lượng hàng hóa cần sắp xếp nhất định.

Game cũng có thể có các tính năng phụ trợ như gợi ý (`Hint`), xáo trộn hàng hóa (`Shuffle`), và các hệ thống thưởng khác.

### 1.2. Chúng ta xây dựng game bằng gì? (Công cụ và Thư viện)

* **Unity Engine:** Đây là "sân chơi" chính của chúng ta. Unity là một công cụ mạnh mẽ, giống như một xưởng phim kỹ thuật số, cho phép chúng ta tạo ra thế giới game, viết kịch bản cho các nhân vật (đối tượng game), và làm cho mọi thứ chuyển động. Bạn sẽ làm việc chủ yếu trong Unity Editor.
* **Ngôn ngữ C# (C-Sharp):** Đây là ngôn ngữ lập trình chúng ta sử dụng để "dạy" cho các đối tượng trong game biết phải làm gì. Hầu hết các file bạn thấy có đuôi `.cs` đều được viết bằng C#.
* **Các "Trợ thủ" Đắc lực - Thư viện:**
    * **DG.Tweening (DOTween):** Giống như một chuyên gia hiệu ứng chuyển động, thư viện này giúp chúng ta tạo ra các cử động mượt mà, đẹp mắt cho hàng hóa khi chúng di chuyển, phóng to, hay mờ đi.
    * **Lofelt.NiceVibrations:** Giúp game "rung" lên một chút trên điện thoại khi có sự kiện đặc biệt xảy ra, ví dụ như khi bạn ghép thành công 3 món hàng. Điều này làm tăng cảm giác thật cho người chơi.
    * **HighlightPlus:** (Có thể đang dùng hoặc thử nghiệm) Giúp làm nổi bật các món hàng khi người chơi chọn hoặc tương tác.
    * **Unity.VisualScripting.FullSerializer:** Một công cụ có thể liên quan đến việc lưu và tải dữ liệu phức tạp.

---

## 2. Tìm hiểu "Bộ Não" của Game: Kiến trúc Hệ thống

Game của chúng ta được cấu thành từ nhiều phần khác nhau, mỗi phần có một nhiệm vụ riêng nhưng tất cả đều phối hợp nhịp nhàng.

### 2.1. Dữ liệu Game được lưu trữ và quản lý như thế nào?

Để game hoạt động, chúng ta cần rất nhiều thông tin: màn chơi này có bao nhiêu kệ? Thời gian bao lâu? Có những loại hàng hóa nào? Tất cả những thứ này gọi là "dữ liệu game".

* **2.1.1. File CSV: Bảng tính chứa thông tin ban đầu**
    * **CSV là gì?** Là viết tắt của "Comma-Separated Values". Hãy tưởng tượng nó như một file Excel đơn giản, nơi mỗi dòng là một bản ghi (ví dụ: thông tin một level), và mỗi cột là một thuộc tính (ví dụ: ID của level, thời gian chơi). Dữ liệu trong các cột được ngăn cách bởi dấu phẩy.
    * **Tại sao dùng CSV?** Vì nó rất dễ đọc, dễ hiểu và dễ chỉnh sửa bằng các chương trình bảng tính như Excel hay Google Sheets. Điều này giúp các bạn thiết kế game (game designer) có thể thay đổi thông số level mà không cần biết code.
    * **Trong dự án:**
        * Thông tin từng màn chơi (level) được định nghĩa trong một file CSV.
        * Thông tin bảng xếp hạng (ví dụ: tên người chơi "ảo" và điểm số của họ) cũng có thể được lưu trong CSV.

* **2.1.2. ScriptableObjects: "Hộp chứa dữ liệu" thông minh trong Unity**
    * **ScriptableObject (SO) là gì?** Đây là một loại "tài sản" (asset) đặc biệt trong Unity. Hãy nghĩ về SO như những chiếc hộp nhỏ, mỗi hộp chứa một bộ dữ liệu cụ thể mà game có thể sử dụng đi sử dụng lại. Ví dụ, một hộp chứa thông tin của Level 1, một hộp khác chứa các cài đặt chung của game.
    * **Tại sao dùng SO?**
        * **Dễ quản lý:** Chúng nằm trong cửa sổ Project của Unity, dễ tìm và chỉnh sửa.
        * **Tái sử dụng:** Nhiều phần của game có thể cùng dùng chung một SO.
        * **Không cần trong Scene:** SO không cần phải đặt vào trong một màn chơi (Scene) cụ thể để hoạt động. Game có thể "gọi" chúng ra từ thư mục Resources khi cần.
    * **Các SO quan trọng trong dự án:**
        * **`LevelData`**: Mỗi file `LevelData.asset` là một "hộp" chứa toàn bộ thông tin cấu hình cho MỘT màn chơi (ví dụ: `Level1.asset`, `Level2.asset`). Nó bao gồm ID màn chơi, ID của cảnh (scene) kệ sẽ dùng, thời gian, tổng số sản phẩm cần tạo, bộ sản phẩm nào sẽ xuất hiện, v.v.
        * **`RankData`**: Một "hộp" chứa danh sách thông tin những người chơi trong bảng xếp hạng (thường là người chơi AI để làm mẫu).
        * **`ProductSet`**: Một "hộp" chứa các "bộ sưu tập" sản phẩm. Mỗi bộ sưu tập có một ID riêng và liệt kê ID của tất cả các loại hàng hóa thuộc bộ đó. Ví dụ, level mùa hè có thể dùng bộ sản phẩm "Đồ đi biển".
        * **`GamePlaySetting`**: "Hộp" này chứa các cài đặt áp dụng cho toàn bộ game, ví dụ như kích thước tối đa của kệ, khoảng cách giữa các ô hàng hóa, số sao cần để mở rương, v.v. Điều này giúp designer dễ dàng "tinh chỉnh" độ khó và trải nghiệm game.

* **2.1.3. Các "Công nhân" Đọc File CSV: Script Parser**
    * **Parser là gì?** Là những đoạn code có nhiệm vụ "đọc hiểu" dữ liệu từ một định dạng (như CSV) và chuyển nó sang một định dạng khác mà máy tính (cụ thể là Unity) có thể sử dụng dễ dàng hơn (như ScriptableObject). Chúng giống như những người phiên dịch.
    * **Trong dự án (Đây là các công cụ dùng trong Unity Editor, không chạy khi người chơi chơi game):**
        * **`LevelParser.cs`**: Script này đọc file CSV chứa thông tin các level. Với mỗi dòng trong CSV, nó tạo ra một đối tượng `LevelDataModel` (một bản nháp tạm thời) rồi từ đó "đúc" ra một file `LevelData.asset` và lưu vào thư mục "Assets/Resources/GameLevels/".
        * **`RankParser.cs`**: Tương tự, script này đọc file CSV chứa dữ liệu bảng xếp hạng, rồi "đúc" ra một file `RankData.asset` (ví dụ "AIRank.asset") và lưu vào "Assets/Resources/Rank/".
        * Cả hai Parser này đều sử dụng một "công cụ phụ" là `CsvParser2` (có thể là một script khác hoặc một phần của thư viện) để giúp chúng tách dữ liệu từ file CSV.
        * **Lưu ý cho Intern:** Bạn có thể thấy trong hàm `Update()` của các Parser này có đoạn code kiểm tra `Input.GetKeyDown(KeyCode.C)`. Đây là một phím tắt tiện lợi: khi bạn chạy game trong Unity Editor và nhấn phím 'C', các Parser này sẽ tự động thực hiện công việc tạo file SO.

### 2.2. Dựng Cảnh Chơi: Tạo ra bàn chơi (Board Generation)

Khi người chơi bắt đầu một level, game cần phải "dựng" lên toàn bộ bàn chơi: các kệ hàng, hàng hóa trên đó, v.v.

* **2.2.1. Ai là "Tổng chỉ huy"? `BoardGenerator.cs`**
    * Đây là một script cực kỳ quan trọng. Nó giống như một đạo diễn sân khấu, điều phối mọi thứ để tạo ra bàn chơi.
    * **Nhiệm vụ chính:**
        * Đọc `LevelData` của màn chơi hiện tại để biết cần những gì.
        * Tải và hiển thị mô hình 3D của các kệ hàng (gọi là "Scene kệ").
        * Quyết định xem sẽ dùng bộ sản phẩm nào (`ProductSet`).
        * **Quan trọng nhất:** Chạy một thuật toán phức tạp gọi là `GenMap()` để sắp xếp một cách ngẫu nhiên (nhưng có kiểm soát) các loại hàng hóa lên từng vị trí trên kệ. Mục tiêu là tạo ra sự đa dạng và thử thách cho mỗi lần chơi.
        * Tạo ra (instantiate) các đối tượng kệ (`ShelfController`) và từng món hàng (`WaresController`) trong thế giới game.
        * Lắng nghe thao tác của người chơi (chạm, kéo, thả hàng hóa) và xử lý chúng.

* **2.2.2. Những chiếc kệ: `ShelfController.cs`**
    * Mỗi đối tượng kệ trong game sẽ có một script `ShelfController` gắn vào. Script này quản lý mọi hoạt động của riêng kệ đó.
    * **Nhiệm vụ:**
        * Khi kệ được tạo, nó sẽ tự "vẽ" ra các vị trí ảo (`warePointList` trong `ShelfSlot`) để biết chính xác chỗ nào có thể đặt hàng hóa. Việc này dựa vào thông số trong `GamePlaySetting`.
        * Lưu giữ thông tin về tất cả các món hàng đang nằm trên nó.
        * Kiểm tra xem 3 món hàng ở hàng đầu tiên có giống nhau không. Nếu có, nó sẽ xử lý việc "ăn điểm" (`ClearFirstRow()`).
        * Khi hàng đầu tiên được xóa, nó sẽ đẩy các hàng hóa ở sâu hơn lên phía trước (`PushRow()`).
        * Có hai loại kệ: `TRIPLE` (3 cột hàng hóa mỗi hàng) và `SINGLE` (1 cột).
    * **`ShelfContainer.cs`**: Đây là một script "phụ" rất đơn giản, chỉ để đánh dấu một GameObject con bên trong kệ. `ShelfController` sẽ tìm GameObject này để biết nơi đặt các "điểm chờ" cho hàng hóa.

* **2.2.3. Từng món hàng: `WaresController.cs`**
    * Mỗi món hàng hóa bạn thấy trên kệ (chai lọ, hộp bánh, v.v.) đều được điều khiển bởi một script `WaresController`.
    * **Nhiệm vụ:**
        * Lưu thông tin về chính nó: Nó là loại hàng gì (`waresID`)? Nó đang nằm ở cột nào (`colID`), hàng sâu thứ mấy (`deptID`) trên kệ nào (`shelfController`)?
        * Quản lý trạng thái của nó: Đang nằm yên (`IDLE`) hay đang được xử lý (`PROCESS`)?
        * Thay đổi vẻ ngoài: Có thể đổi màu để trông "ẩn" đi nếu nằm sâu, hoặc bật/tắt hoàn toàn việc hiển thị.
        * Thực hiện các hành động: Di chuyển đến vị trí mới, quay về chỗ cũ, hoặc "biến mất" khỏi game. Các chuyển động này thường rất mượt mà nhờ DOTween.

* **2.2.4. "Công thức" tạo map: `GenMap` trong `BoardGenerator.cs`**
    * Đây là một phần rất "ảo diệu" của game. Thay vì designer phải tự tay xếp từng món hàng cho hàng trăm level, `GenMap` sẽ làm điều đó một cách tự động.
    * **Ý tưởng:** Nó không xếp hoàn toàn ngẫu nhiên đến mức hỗn loạn, mà tuân theo một số quy tắc để đảm bảo level vừa thử thách vừa có thể giải được.
    * **Các bước chính (đơn giản hóa):**
        1.  **Đếm hàng:** Dựa vào `totalProductCount` của level, tính xem cần bao nhiêu bộ 3 của mỗi loại sản phẩm.
        2.  **Tạo "kho":** Tạo ra các "kho" (`ProductPool`) chứa đủ số lượng ID của từng loại sản phẩm (và cả các "ID ảo" cho ô trống).
        3.  **Rải hàng lên bản đồ nháp:** Lấy các ID sản phẩm từ "kho" và đặt chúng vào một danh sách lớn gọi là `originalTileList` (đại diện cho tất cả các ô trên tất cả các kệ). Việc này có tính đến việc tạo ô trống ở những vị trí chiến lược.
        4.  **Xáo trộn:** Để mỗi lần chơi có khác biệt, danh sách này sẽ được xáo trộn ở nhiều cấp độ: xáo trộn 3 vị trí trong một tile, xáo trộn các tile trong một cột, v.v.
        5.  **Chia về từng kệ:** Cuối cùng, danh sách đã xáo trộn này được tổ chức lại thành `cellListData`, là dữ liệu chi tiết cho từng kệ hàng. `BoardGenerator` sẽ dựa vào đây để thực sự tạo ra các đối tượng hàng hóa.

### 2.3. Cách Người Chơi Tương Tác: Gameplay Chính

* **2.3.1. Nhặt và đặt hàng hóa**
    * Trong hàm `Update()` của `BoardGenerator.cs`, game liên tục kiểm tra xem người chơi có chạm vào màn hình không.
    * Nếu người chơi chạm vào một món hàng ở hàng đầu tiên của kệ (`deptID == 0`) và món hàng đó đang "rảnh" (`currentState == WaresController.STATE.IDLE`), món hàng đó sẽ được "nhấc" lên và di chuyển theo ngón tay của người chơi.
    * Khi người chơi thả ngón tay:
        * Game sẽ kiểm tra xem có kệ nào bên dưới không.
        * Nếu có kệ `TRIPLE` và kệ đó có ô trống ở hàng đầu tiên, món hàng sẽ được đặt vào đó.
        * Nếu không, hoặc nếu là kệ `SINGLE`, món hàng sẽ bay về vị trí cũ của nó.

* **2.3.2. "Ăn điểm"! Ghép 3 món giống nhau**
    * Mỗi khi một món hàng được đặt xuống một kệ, hoặc khi một kệ bị đẩy hàng lên, `ShelfController` của kệ đó sẽ gọi hàm `CheckFirstRow()`.
    * Hàm này kiểm tra xem 3 món hàng ở hàng đầu tiên (nếu là kệ `TRIPLE`) có cùng `waresID` (cùng loại) hay không.
    * Nếu có, hàm `ClearFirstRow()` sẽ được gọi:
        * 3 món hàng đó sẽ thực hiện một animation đẹp mắt rồi biến mất.
        * Người chơi nhận được điểm, có thể có hiệu ứng âm thanh, rung, và hình ảnh đặc biệt.
        * Sau đó, hàm `PushRow()` được gọi để các món hàng ở sâu hơn trên kệ đó được đẩy lên một hàng.

* **2.3.3. Trợ giúp nhỏ: Hint và Shuffle**
    * `BoardGenerator.cs` cũng quản lý các tính năng này:
        * **Hint (`ProcessHint()`):** Nếu người chơi "bí", game có thể tìm và chỉ ra một bộ 3 món hàng có thể ghép được, rồi tự động "ăn" chúng.
        * **Shuffle (`ProcessShuffle()`):** Xáo trộn vị trí các hàng (`ShelfSlot`) bên trong mỗi kệ, tạo ra một tình huống mới.

### 2.4. Ai là Số Một? Hệ thống Xếp hạng
* Dữ liệu về thứ hạng (ví dụ: tên người chơi và điểm số cao) được lưu trong `RankData.asset` (tạo từ CSV bởi `RankParser.cs`).
* Các cài đặt liên quan đến việc lên hạng, thưởng khi lên hạng, số người hiển thị trên bảng xếp hạng được định nghĩa trong `GamePlaySetting.asset`.
* (Logic cụ thể về cách người chơi thật sự leo rank và hiển thị bảng xếp hạng có thể nằm ở các script khác chưa được cung cấp, nhưng nền tảng dữ liệu đã sẵn sàng.)

### 2.5. Cho Game Thêm "Lấp Lánh": Hiệu ứng Hình ảnh (VFX) và Âm thanh
* **`ClearVfx.cs`**: Đây là một script điều khiển một hiệu ứng hình ảnh đơn giản. Khi 3 món hàng được "ăn", một đối tượng có gắn script này sẽ được kích hoạt (`PlayAnim()`). Nó sẽ hiển thị một hình ảnh (sprite), làm nó phóng to, xoay tròn rồi mờ dần đi. Script này được gọi từ `ShelfController` thông qua một tham chiếu có lẽ là `GameManager.Instance.clearVfx`.
* Tương tự, `GameManager.Instance.comboVfx` cũng được dùng để tạo hiệu ứng khi có combo.
* **`Star.cs`**: Script này có vẻ dùng để điều khiển việc hiển thị và di chuyển các ngôi sao trên màn hình UI (User Interface - Giao diện người dùng). Ví dụ, khi người chơi làm tốt, sao có thể bay đến một vị trí nào đó. `ShelfController` gọi `GameManager.Instance.starRoot.SpawnStar()` khi có điểm.
* **Âm thanh & Rung:** Game sử dụng `AudioManager.instance` để phát các âm thanh như tiếng chọn đồ, tiếng "ăn điểm". Nó cũng dùng `HapticPatterns.PlayPreset()` để tạo hiệu ứng rung trên điện thoại.

### 2.6. "Người Duy Nhất": Lớp Singleton - Quản lý Toàn cục
* **Singleton là gì?** Là một mẫu thiết kế trong lập trình, đảm bảo rằng một lớp nào đó chỉ có DUY NHẤT MỘT đối tượng (instance) được tạo ra trong toàn bộ chương trình. Giống như trong một vương quốc chỉ có một vị vua vậy.
* **`Singleton.cs`**: Đây là một script "mẫu" dùng để dễ dàng biến một script khác thành Singleton.
* **Tại sao dùng?** Rất hữu ích cho các "Quản lý viên" toàn cục của game, ví dụ:
    * `GameManager`: Quản lý trạng thái chung của game (đang chơi, thua, thắng), điểm số, level hiện tại, v.v.
    * `AudioManager`: Quản lý việc phát tất cả âm thanh trong game.
    * Bất kỳ script nào khác có `Instance` (ví dụ `GameManager.Instance`, `AudioManager.instance`) đều có thể đang sử dụng mô hình Singleton này. Điều này cho phép các script khác dễ dàng truy cập đến các "Quản lý viên" này từ bất cứ đâu mà không cần truyền tham chiếu phức tạp.

---

## 3. "Hồ sơ Nhân sự" của các Script Chính: Tìm hiểu từng File Code

Ở phần này, chúng ta sẽ điểm lại các script quan trọng và vai trò của chúng, cùng với một vài gợi ý cho bạn khi làm việc với chúng. (Một số thông tin sẽ lặp lại từ phần 2 để củng cố kiến thức).

* **`LevelData.cs`**
    * **Nó là gì?** Một ScriptableObject, như một "thẻ thông tin" cho mỗi màn chơi.
    * **Nó làm gì?** Chứa các thông số như ID màn, ID scene kệ, thời gian, số sản phẩm, bộ sản phẩm dùng, v.v.
    * **Điểm cần chú ý cho Intern:** Nếu bạn cần thêm một thuộc tính mới cho tất cả các level (ví dụ: "số lượt gợi ý tối đa"), bạn sẽ cần thêm một biến public vào script này. Sau đó, `LevelParser.cs` cũng cần được cập nhật để đọc giá trị đó từ CSV.

* **`LevelParser.cs`** (và lớp con `LevelDataModel`)
    * **Nó là gì?** Một `MonoBehaviour` (script có thể gắn vào GameObject trong Scene), hoạt động như một công cụ trong Unity Editor.
    * **Nó làm gì?** Đọc file CSV chứa dữ liệu các level, "phiên dịch" từng dòng thành một `LevelDataModel` (đối tượng C# tạm thời), rồi tạo ra các file `LevelData.asset` tương ứng. Nó có một hàm `ParseData()` để thực hiện việc này.
    * **Điểm cần chú ý cho Intern:**
        * Nếu cấu trúc file CSV thay đổi (thêm cột, đổi thứ tự cột), bạn cần cập nhật logic trong `ParseData()` để đọc đúng cột cho đúng thuộc tính. Ví dụ, nếu cột "Thời gian" chuyển từ cột 3 sang cột 4 trong CSV, bạn phải sửa `lv.time = int.Parse(dataArr[i][3]);` thành `lv.time = int.Parse(dataArr[i][4]);`.
        * Cẩn thận với việc bỏ qua 4 dòng đầu tiên (`if (i >= 4)`), đây là giả định về số dòng tiêu đề trong CSV.

* **`RankData.cs`** (và lớp con `RankDataModel`)
    * **Nó là gì?** ScriptableObject chứa danh sách thông tin bảng xếp hạng.
    * **Nó làm gì?** `RankDataModel` định nghĩa cấu trúc của một entry rank (ID, tên, điểm). `RankData` chứa một `List<RankDataModel>`.
    * **Điểm cần chú ý cho Intern:** Tương tự `LevelData`, nếu cần thêm thông tin cho mỗi entry rank, bạn sửa `RankDataModel`.

* **`RankParser.cs`**
    * **Nó là gì?** Công cụ Editor, tương tự `LevelParser` nhưng cho dữ liệu rank.
    * **Nó làm gì?** Đọc CSV rank, tạo `RankData.asset`.
    * **Điểm cần chú ý cho Intern:**
        * Tên biến `levelCsvFile` trong script này hiện đang dùng để chỉ file CSV rank, hơi dễ nhầm lẫn. Có thể đổi thành `rankCsvFile` cho rõ ràng hơn.
        * Logic parse trong `ParseData()` cũng phụ thuộc vào thứ tự cột trong CSV rank.

* **`ProductSet.cs`** (và lớp con `ProductItem`)
    * **Nó là gì?** ScriptableObject định nghĩa các "bộ sưu tập" sản phẩm.
    * **Nó làm gì?** `ProductItem` có ID bộ và danh sách ID các sản phẩm (wares) thuộc bộ đó. `ProductSet` chứa danh sách các `ProductItem`.
    * **Điểm cần chú ý cho Intern:** Khi designer muốn tạo một nhóm sản phẩm mới cho một chủ đề (ví dụ: "Đồ Giáng Sinh"), họ sẽ tạo một `ProductItem` mới trong `ProductSet.asset` và điền ID của các sản phẩm Giáng Sinh vào đó.

* **`GamePlaySetting.cs`**
    * **Nó là gì?** ScriptableObject chứa các cài đặt chung của game.
    * **Nó làm gì?** Cho phép designer dễ dàng tinh chỉnh các thông số như kích thước kệ, khoảng cách ô, các mốc thưởng, điểm rank, v.v., mà không cần đụng vào code.
    * **Điểm cần chú ý cho Intern:** Nếu có một thông số mới cần cho toàn bộ game (ví dụ: "tốc độ rơi của item gợi ý"), bạn sẽ thêm một biến public vào đây. Designer có thể chỉnh giá trị đó trực tiếp trên file `.asset` trong Unity.

* **`BoardGenerator.cs`** (và các lớp lồng nhau `ProductPool`, `ShelfTile`, `ShelfCell`, `HintItem`)
    * **Nó là gì?** "Trái tim" của gameplay, một `MonoBehaviour` điều phối việc tạo bàn chơi và xử lý tương tác.
    * **Nó làm gì?** Rất nhiều việc! Từ tải dữ liệu level, sản phẩm, sinh map một cách thuật toán (`GenMap` và các hàm liên quan như `InitPairItems`, `LoadItemsToPool`, `LoadTiles`, `FillData`, các hàm `Shuffle...`, `Mix...`), tạo đối tượng game, đến xử lý input của người chơi (`Update`, `IsHitWares`, `FindHittedShelf`) và kích hoạt các tính năng phụ (`ProcessHint`, `ProcessShuffle`).
    * **Điểm cần chú ý cho Intern:**
        * Phần `GEN_MAP_FUNCTION` là cực kỳ quan trọng và phức tạp. Nếu bạn được giao nhiệm vụ thay đổi cách map được sinh ra (ví dụ: muốn đảm bảo luôn có ít nhất 2 cặp có thể ăn được ở lượt đầu), bạn sẽ cần nghiên cứu kỹ vùng này.
        * Logic trong `Update()` xử lý việc người chơi chọn và di chuyển hàng hóa.
        * Các hằng số như `100000` cho `dynamicProductSet` để chọn random sản phẩm là một "magic number". Nếu có thể, nên định nghĩa chúng thành hằng số có tên rõ ràng (ví dụ: `const int RANDOM_PRODUCT_SET_ID = 100000;`).

* **`ShelfController.cs`** (và lớp lồng nhau `ShelfSlot`)
    * **Nó là gì?** `MonoBehaviour` quản lý một chiếc kệ riêng lẻ.
    * **Nó làm gì?** Tạo các điểm đặt hàng (`GenerateWarePoint`), lưu trữ hàng hóa (`shelfSlotList`), kiểm tra xem có 3 món hàng giống nhau ở hàng đầu không (`CheckFirstRow`), và xử lý khi chúng được "ăn" (`ClearFirstRow`, `PushRow`).
    * **Điểm cần chú ý cho Intern:**
        * Logic `CleanEmptyRow` và `RePosition` rất quan trọng để kệ luôn hiển thị đúng sau khi hàng hóa bị xóa hoặc di chuyển.
        * Cách `deptSize` được xác định (`GameManager.Instance.gameBoard.shelfDeepMax`) cho thấy sự phụ thuộc vào `BoardGenerator` để biết chiều sâu của kệ trong level hiện tại.

* **`WaresController.cs`**
    * **Nó là gì?** `MonoBehaviour` điều khiển một món hàng hóa.
    * **Nó làm gì?** Lưu thông tin (`waresID`, `colID`, `deptID`), quản lý trạng thái, màu sắc, hiển thị, và thực hiện các hành động di chuyển (`MoveBack`, `MoveToAnotherShelf`) hoặc bị xóa (`RemoveItem`).
    * **Điểm cần chú ý cho Intern:**
        * Hàm `InitWares()` lấy các component cần thiết như `MeshRenderer` từ con của nó (nếu có) hoặc từ chính nó. Điều này quan trọng khi bạn thiết lập prefab cho hàng hóa.
        * Việc sử dụng `DOTween` cho các hàm `MoveBack` và `MoveToAnotherShelf` giúp chuyển động mượt mà.

* **`ShelfContainer.cs`**
    * **Nó là gì?** Một script `MonoBehaviour` rất đơn giản, gần như không có code.
    * **Nó làm gì?** Chỉ để "đánh dấu" một GameObject con bên trong prefab của kệ. `ShelfController` sẽ tìm đến GameObject này để biết nơi nào sẽ là "cha" của các điểm đặt hàng hóa.
    * **Điểm cần chú ý cho Intern:** Khi tạo prefab cho một kệ mới, đừng quên tạo một GameObject con và gắn script `ShelfContainer` này vào đó, rồi đặt nó làm con đầu tiên của kệ.

* **`ClearVfx.cs`**
    * **Nó là gì?** `MonoBehaviour` điều khiển một hiệu ứng hình ảnh.
    * **Nó làm gì?** Khi được gọi (`PlayAnim`), nó sẽ hiển thị một sprite, làm nó phóng to, xoay, mờ dần bằng `DOTween`. Có một mảng `spriteArr` để có thể chọn ngẫu nhiên sprite cho hiệu ứng.
    * **Điểm cần chú ý cho Intern:** Nếu muốn thay đổi cách hiệu ứng này hoạt động (ví dụ: đổi tốc độ, kiểu di chuyển), bạn sẽ sửa hàm `PlayAnim`.

* **`Singleton.cs`**
    * **Nó là gì?** Một script `MonoBehaviour` dùng chung, giúp tạo ra các lớp "Quản lý viên" toàn cục.
    * **Nó làm gì?** Đảm bảo chỉ có một instance của một lớp (ví dụ `GameManager`) tồn tại. Cung cấp một cách dễ dàng để truy cập instance đó từ bất kỳ đâu thông qua `T.Instance`.
    * **Điểm cần chú ý cho Intern:** Khi bạn thấy một script gọi `GameManager.Instance` hoặc `AudioManager.Instance`, đó là nhờ vào lớp Singleton này. Nếu bạn cần tạo một "Quản lý viên" mới cho toàn game, bạn có thể cho nó kế thừa từ `Singleton<TênLớpQuảnLýCủaBạn>`.

* **`Star.cs`**
    * **Nó là gì?** `MonoBehaviour` có vẻ dùng để quản lý một đối tượng "ngôi sao" trên giao diện người dùng (UI).
    * **Nó làm gì?** Chứa hàm `WorldToCanvasPosition` để chuyển đổi một vị trí trong thế giới 3D của game thành một vị trí trên màn hình UI (Canvas). Điều này hữu ích nếu bạn muốn một ngôi sao bay từ vị trí hàng hóa vừa được "ăn" đến thanh điểm số chẳng hạn.
    * **Điểm cần chú ý cho Intern:** Script này có thể là một phần của một hệ thống lớn hơn quản lý việc hiển thị sao thưởng, được gọi qua `GameManager.Instance.starRoot.SpawnStar()`.

---

## 4. Dòng Chảy Công Việc và Dữ Liệu: Từ Ý Tưởng Đến Gameplay

Hiểu được dữ liệu di chuyển như thế nào sẽ giúp bạn hình dung rõ hơn về cách các thành phần tương tác.

### 4.1. Hành trình của Dữ liệu Màn chơi (Level)

1.  **Ý tưởng (Designer):** Designer nghĩ ra một màn chơi mới: "Level 5 sẽ có chủ đề rừng rậm, thời gian 90 giây, cần ghép 30 bộ sản phẩm, sử dụng bộ sản phẩm 'Trái cây rừng'."
2.  **Nhập liệu (Designer):**
    * Mở file `Level.csv` (ví dụ: `GameLevels.csv`).
    * Thêm một dòng mới, điền các thông tin: `ID=5`, `sceneID=2` (giả sử sceneID 2 là prefab kệ kiểu rừng rậm), `time=90`, `totalProductCount=30`, `dynamicProductSet=102` (giả sử ID 102 là bộ "Trái cây rừng"), `roundsEmptyPlaceCount=2`, v.v.
3.  **"Nạp" vào Unity (Developer/Designer trong Unity Editor):**
    * Trong Unity, mở Scene có chứa GameObject đã gắn script `LevelParser.cs`.
    * Đảm bảo file `Level.csv` đã được kéo vào dưới dạng `TextAsset` và gán cho biến `levelCsvFile` của `LevelParser`.
    * Chạy Scene này trong Editor, rồi nhấn phím 'C'.
4.  **Phép màu xảy ra (`LevelParser.cs`):**
    * Script `LevelParser` sẽ đọc từng dòng trong `Level.csv`.
    * Với dòng của Level 5, nó sẽ lấy các giá trị ra.
    * Nó tạo ra một file mới tên là `Level5.asset` (đây là một `LevelData` ScriptableObject) trong thư mục "Assets/Resources/GameLevels/". File này chứa tất cả thông tin của Level 5.
5.  **Sẵn sàng để chơi (`BoardGenerator.cs` khi runtime):**
    * Khi người chơi chọn chơi Level 5:
    * `BoardGenerator` sẽ tìm và tải file `Level5.asset` từ thư mục Resources.
    * Nó đọc thông tin từ `Level5.asset` để biết cần dựng bàn chơi như thế nào (dùng scene kệ nào, bộ sản phẩm nào, v.v.).

### 4.2. Hành trình của Dữ liệu Sản phẩm (Wares)

1.  **Sáng tạo (Artist/3D Modeler):** Tạo ra mô hình 3D cho một món hàng mới, ví dụ "Quả Dâu Rừng".
2.  **Chuẩn bị Prefab (Artist/Designer trong Unity Editor):**
    * Import model 3D vào Unity.
    * Tạo một "Prefab" từ model này. Prefab giống như một "bản thiết kế" cho đối tượng. Đặt tên prefab theo ID, ví dụ `1001.prefab`.
    * Đặt prefab này vào đúng đường dẫn: "Assets/Resources/Wares/{ID_Sản_Phẩm}/{ID_Sản_Phẩm}.prefab". (ID sản phẩm là 1001).
    * Gắn script `WaresController.cs` vào prefab này.
    * Thiết lập các thành phần cần thiết trên prefab (ví dụ `MeshRenderer` cho hình ảnh).
3.  **Định nghĩa Bộ Sản Phẩm (Designer trong Unity Editor):**
    * Tìm và chọn file `ProductSet.asset` trong cửa sổ Project.
    * Trong Inspector, tìm đến `productSetList`.
    * Tạo một `ProductItem` mới (ví dụ, đặt tên là "Bộ Trái Cây Rừng", ID là 102) hoặc chọn một `ProductItem` đã có.
    * Trong `productList` của `ProductItem` đó, thêm ID của "Quả Dâu Rừng" (là `1001`).
4.  **Sử dụng trong Game (`BoardGenerator.cs` khi runtime):**
    * Khi một level (ví dụ Level 5 ở trên) yêu cầu dùng bộ sản phẩm có ID 102 (`currentLv.dynamicProductSet == 102`):
    * `BoardGenerator` sẽ đọc `ProductSet.asset`, tìm `ProductItem` có ID 102.
    * Nó thấy ID `1001` ("Quả Dâu Rừng") trong danh sách.
    * Nó sẽ tải prefab "Assets/Resources/Wares/1001/1001.prefab" để tạo ra các đối tượng "Quả Dâu Rừng" trong game.

### 4.3. "Cẩm nang" cho Công việc Hàng ngày của bạn

Đây là các bước chi tiết cho những công việc bạn có thể được giao.

* **4.3.1. Cách thêm một Level mới hoặc sửa Level cũ**
    1.  **Mở file CSV:** Tìm file CSV chứa dữ liệu level (ví dụ: `ProjectFolder/Data/GameLevels.csv`). Mở bằng Excel, Google Sheets hoặc một trình soạn thảo văn bản hỗ trợ CSV.
    2.  **Thêm/Sửa dòng:**
        * **Để thêm level mới:** Cuộn xuống cuối file, thêm một dòng mới.
        * **Để sửa level cũ:** Tìm đến dòng của level bạn muốn sửa.
    3.  **Điền thông tin các cột:** Dựa vào các cột đã có và ý nghĩa của chúng (được định nghĩa trong `LevelParser.ParseData()`):
        * **Cột 0 (ID):** ID duy nhất cho level (ví dụ: `101`).
        * **Cột 1 (sceneID):** ID của prefab Scene kệ sẽ dùng (ví dụ: `1`, `2`). Số này phải tương ứng với một prefab trong "Assets/Resources/Shelves/{sceneID}/Scene{sceneID}.prefab".
        * **Cột 3 (time):** Thời gian hoàn thành màn chơi (tính bằng giây).
        * **Cột 6 (totalProductCount):** Tổng số "bộ 3" sản phẩm cần tạo trong màn. Ví dụ, nếu là 24, game sẽ cố gắng tạo 24 * 3 = 72 sản phẩm.
        * **Cột 8 (dynamicProductSet):** ID của bộ sản phẩm (`ProductItem` ID trong `ProductSet.asset`) sẽ dùng. Nhập `100000` nếu muốn game chọn ngẫu nhiên một bộ sản phẩm.
        * **Cột 12 (singlePush):** Có thể là chiều sâu tối đa ban đầu của kệ hoặc một giá trị liên quan đến kệ đẩy đơn.
        * **Cột 13 (roundsEmptyPlaceCount):** Số ô trống sẽ được tạo ở hàng đầu tiên của các kệ ở vòng đầu. Có thể có định dạng `số|chữgìđó`, nhưng code hiện tại chỉ lấy phần `số`.
        * *Lưu ý: Các chỉ số cột (0, 1, 3,...) là 0-based index.*
    4.  **Lưu file CSV.**
    5.  **Chạy Parser trong Unity:**
        * Mở Unity Editor.
        * Mở Scene chứa GameObject đã gắn script `LevelParser.cs` (ví dụ một Scene tên là "EditorTools_Scene").
        * Chạy Scene đó (Nhấn nút Play).
        * Trong khi Scene đang chạy, nhấn phím 'C' trên bàn phím.
        * Dừng Scene (Nhấn lại nút Play).
    6.  **Kiểm tra:** Vào thư mục "Assets/Resources/GameLevels/" trong cửa sổ Project của Unity. Bạn sẽ thấy file `Level<ID_Mới>.asset` đã được tạo hoặc file level cũ đã được cập nhật.
    7.  **Kiểm tra Prefab Kệ:** Đảm bảo rằng `sceneID` bạn nhập ở bước 3 tương ứng với một prefab kệ đã tồn tại trong "Assets/Resources/Shelves/". Nếu không, bạn cần tạo prefab kệ đó hoặc dùng `sceneID` của một kệ đã có. Prefab kệ này cần có các `ShelfController` và `ShelfContainer` được thiết lập đúng.

* **4.3.2. Cách thêm một Loại Sản Phẩm (Wares) mới**
    1.  **Tạo Model 3D & Texture:** (Thường do Artist làm) Tạo model 3D và texture cho sản phẩm mới.
    2.  **Tạo Prefab:**
        * Import model và texture vào Unity.
        * Tạo một Prefab mới từ model đó. Đặt tên Prefab là ID của sản phẩm (ví dụ: `1005.prefab`). ID này phải là duy nhất.
        * Đặt Prefab vào thư mục: "Assets/Resources/Wares/{ID_Sản_Phẩm}/{ID_Sản_Phẩm}.prefab". Ví dụ: "Assets/Resources/Wares/1005/1005.prefab".
        * **Quan trọng:** Gắn script `WaresController.cs` vào Prefab này.
        * Đảm bảo Prefab có `MeshRenderer` để hiển thị model. Nếu model có nhiều phần, `WaresController` sẽ tìm `MeshRenderer` ở con đầu tiên hoặc ở chính nó.
    3.  **Thêm vào `ProductSet.asset`:**
        * Trong cửa sổ Project của Unity, tìm và chọn file `ProductSet.asset` (thường nằm trong thư mục Resources hoặc một thư mục Data).
        * Trong cửa sổ Inspector, bạn sẽ thấy `Product Set List`.
        * **Cách 1: Thêm vào bộ hiện có:** Chọn một `Product Item` trong danh sách. Trong phần `Product List` của item đó, tăng kích thước (Size) lên 1 và điền ID sản phẩm mới (ví dụ: `1005`) vào ô mới.
        * **Cách 2: Tạo bộ mới:** Tăng kích thước của `Product Set List` lên 1. Một `Product Item` mới sẽ xuất hiện. Đặt `ID` cho bộ mới này (ví dụ: `103`). Trong `Product List` của bộ mới, thêm ID sản phẩm mới (ví dụ: `1005`).
    4.  **Sử dụng trong Level:** Bây giờ, khi thiết kế level trong file CSV, bạn có thể dùng ID của bộ sản phẩm (`dynamicProductSet`) vừa cập nhật (ví dụ `103`) để level đó sử dụng sản phẩm mới này.

* **4.3.3. Cách thay đổi Cài đặt Chung của Game**
    1.  **Tìm file `GamePlaySetting.asset`:** Trong cửa sổ Project của Unity, tìm file này (thường nằm trong thư mục Resources hoặc Data).
    2.  **Chọn file:** Nhấp chuột vào file `GamePlaySetting.asset`.
    3.  **Chỉnh sửa trong Inspector:** Cửa sổ Inspector sẽ hiển thị tất cả các cài đặt công khai (public) của `GamePlaySetting.cs`.
        * Ví dụ, để thay đổi kích thước ô mặc định, bạn sửa các giá trị `Tile Size X`, `Tile Size Y`, `Tile Size Z`.
        * Để thay đổi số sao cần để mở rương, sửa `Open Chest Number`.
    4.  **Lưu Project:** Các thay đổi trên ScriptableObject thường được lưu tự động, nhưng bạn nên nhấn Ctrl+S (hoặc Cmd+S trên Mac) để chắc chắn.
    5.  Những thay đổi này sẽ có hiệu lực ngay lập tức trong game khi bạn chạy lại (vì `GameManager.Instance.gamePlaySetting` sẽ tham chiếu đến file `.asset` đã cập nhật).

---

## 5. "Từ điển" Các Thuật Ngữ và Cấu Trúc Dữ Liệu Quan Trọng

* **`MonoBehaviour`**: Trong Unity, đây là lớp cơ sở cho tất cả các script có thể gắn vào một `GameObject` (đối tượng trong game). Các script `MonoBehaviour` có các hàm đặc biệt được Unity tự động gọi như `Start()` (gọi một lần khi đối tượng được tạo), `Update()` (gọi mỗi khung hình). Hầu hết các script chính của chúng ta (`BoardGenerator`, `ShelfController`, `WaresController`) đều là `MonoBehaviour`.
* **`ScriptableObject`**: Như đã nói, đây là các "hộp chứa dữ liệu" dưới dạng asset trong Project. Chúng không cần nằm trong Scene. `LevelData`, `RankData`, `ProductSet`, `GamePlaySetting` là các ví dụ.
* **`GameObject`**: Là một "thứ" bất kỳ trong thế giới game của bạn: một nhân vật, một cái cây, một cái kệ, một món hàng, một nguồn sáng, camera, v.v.
* **`Component`**: Là các "khối chức năng" bạn gắn vào `GameObject` để mang lại cho nó hành vi hoặc đặc tính. Một script `MonoBehaviour` là một loại component. `Transform` (quản lý vị trí, xoay, kích thước), `MeshRenderer` (hiển thị model 3D) cũng là component.
* **`Transform`**: Mọi `GameObject` đều có một `Transform` component. Nó xác định vị trí (position), góc xoay (rotation), và kích thước (scale) của `GameObject` đó trong thế giới game.
* **`Prefab`**: Là một "bản thiết kế" đã lưu của một `GameObject` hoàn chỉnh (bao gồm tất cả các component và cài đặt của nó). Bạn có thể tạo nhiều "bản sao" (instance) từ một Prefab. Các món hàng, các loại kệ có thể là Prefab.
* **`Instantiate()`**: Lệnh trong code Unity dùng để tạo một bản sao mới của một Prefab (hoặc một `GameObject` khác) vào trong Scene đang chạy. `BoardGenerator` dùng lệnh này rất nhiều để tạo kệ và hàng hóa.
* **`Resources` (thư mục):** Một thư mục đặc biệt trong "Assets". Bất cứ thứ gì (Prefab, ScriptableObject, Texture,...) đặt trong thư mục "Resources" (hoặc thư mục con của nó) đều có thể được tải vào game lúc đang chạy bằng lệnh `Resources.Load("Đường/Dẫn/TênFile")`. Dự án này dùng nó để tải `LevelData`, prefab Scene kệ, prefab hàng hóa.
* **Các cấu trúc dữ liệu trong code:**
    * **`LevelDataModel`**: Một lớp C# đơn giản (không phải `MonoBehaviour` hay `ScriptableObject`) được dùng bởi `LevelParser` để tạm thời giữ thông tin một level khi đọc từ CSV, trước khi tạo thành `LevelData.asset`.
    * **`RankDataModel`**: Tương tự, dùng cho `RankParser` và `RankData`.
    * **`ProductItem`**: Bên trong `ProductSet.cs`, định nghĩa một bộ sản phẩm với ID và danh sách ID sản phẩm con.
    * **`ShelfSlot`**: Bên trong `ShelfController.cs`, đại diện cho một hàng trên kệ. Nó chứa một danh sách các `WaresController` (các món hàng đang ở hàng đó) và một danh sách các `Transform` (các điểm đặt chính xác cho từng món hàng).
    * **`ProductPool`**: Bên trong `BoardGenerator.cs`, dùng trong thuật toán sinh map. Mỗi `ProductPool` giữ ID của một loại sản phẩm và một danh sách các ID đó (để biết còn bao nhiêu sản phẩm loại đó có thể "rút" ra).
    * **`ShelfTile`**: Bên trong `BoardGenerator.cs`, cũng dùng trong sinh map. Một `ShelfTile` thường chứa 3 vị trí (ID sản phẩm) cho một phần của hàng trên kệ.
    * **`ShelfCell`**: Bên trong `BoardGenerator.cs`, là kết quả cuối của việc tổ chức dữ liệu map. Mỗi `ShelfCell` đại diện cho toàn bộ dữ liệu (các `ShelfTile` theo chiều sâu) của một kệ.

---

## 6. "Đồng Minh" Từ Bên Ngoài: Các Thư Viện Sử Dụng

Nhắc lại về các thư viện, chúng giúp chúng ta tiết kiệm rất nhiều thời gian và công sức:

* **DOTween (`DG.Tweening`):** Giúp các đồ vật di chuyển, thay đổi kích thước, mờ dần một cách mượt mà và dễ dàng lập trình. Thay vì phải tự viết code phức tạp cho animation, bạn chỉ cần gọi một vài lệnh của DOTween.
* **Lofelt Nice Vibrations:** Làm cho điện thoại rung lên khi có sự kiện quan trọng, tăng trải nghiệm người dùng.
* **Highlight Plus:** (Nếu được sử dụng) Giúp làm nổi bật các đối tượng được chọn.
