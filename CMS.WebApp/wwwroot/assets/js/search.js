// searchProduct.js

$(document).ready(function () {
    // Bắt sự kiện khi người dùng nhập vào ô tìm kiếm
    $('input[name="keyword"]').on('input', function () {
        var keyword = $(this).val().trim(); // Lấy giá trị từ input tìm kiếm

        // Nếu không có gì trong ô tìm kiếm thì dừng lại
        if (keyword.length === 0) {
            $('#searchResults').hide(); // Ẩn danh sách gợi ý
            return;
        }

        // Gửi request tìm kiếm qua API đã có (Sử dụng /api/products/search từ controller)
        $.ajax({
            url: '/api/products/search',  // Đường dẫn tới API tìm kiếm sản phẩm
            type: 'GET',
            data: { keyword: keyword },  // Truyền từ khóa tìm kiếm vào API
            success: function (response) {
                if (response && response.length > 0) {
                    var resultsHtml = '';
                    response.forEach(function (product) {
                        // Tạo các dòng kết quả tìm kiếm
                        resultsHtml += `<li><a href="#" data-product-id="${product.productId}">${product.productName} (${product.barcode})</a></li>`;
                    });

                    // Hiển thị kết quả tìm kiếm dưới ô input
                    $('#searchResults').html(resultsHtml).show();
                } else {
                    $('#searchResults').hide();  // Ẩn nếu không có kết quả
                }
            },
            error: function () {
                $('#searchResults').hide();  // Ẩn nếu có lỗi xảy ra
            }
        });
    });

    // Khi người dùng chọn sản phẩm từ gợi ý, hiển thị thông tin
    $(document).on('click', '#searchResults a', function (e) {
        e.preventDefault();

        var productId = $(this).data('product-id');
        var productName = $(this).text();

        // Gán thông tin sản phẩm vào bảng (ví dụ bạn có bảng sản phẩm nhập kho)
        var newRow = `<tr>
                        <td>
                            <select class="form-control product-select" name="Items[0].ProductId" required>
                                <option value="${productId}">${productName}</option>
                            </select>
                        </td>
                        <td><input type="number" class="form-control quantity-input" name="Items[0].Quantity" value="1" min="1" required></td>
                        <td><input type="number" class="form-control costprice-input" name="Items[0].CostPrice" value="0" min="0" step="0.01" required></td>
                        <td><input type="date" class="form-control expirationdate-input" name="Items[0].ExpirationDate"></td>
                        <td><button type="button" class="btn btn-danger btn-sm remove-product-row">Xóa</button></td>
                    </tr>`;

        $('#productDetailsBody').append(newRow);
        $('#searchResults').hide();  // Ẩn kết quả sau khi chọn
    });

    // Khi người dùng xóa sản phẩm khỏi bảng
    $(document).on('click', '.remove-product-row', function () {
        $(this).closest('tr').remove();
    });
});
