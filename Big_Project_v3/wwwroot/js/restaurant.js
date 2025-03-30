//錨點變動
document.addEventListener('DOMContentLoaded', function () {
    const navLinks = document.querySelectorAll('.nav-link');

    const setActiveLink = () => {
        const hash = window.location.hash;
        navLinks.forEach(link => {
            link.classList.remove('active');
            if (link.getAttribute('href') === hash) {
                link.classList.add('active');
            }
        });
    };

    // 初始設置選中狀態
    setActiveLink();

    // 點擊時設置選中狀態
    navLinks.forEach(link => {
        link.addEventListener('click', function () {
            setActiveLink();
        });
    });

    // 當 URL 哈希值變化時設置選中狀態
    window.addEventListener('hashchange', setActiveLink);
});



//環境照片
document.addEventListener('DOMContentLoaded', function () {
    const imageElements = document.querySelectorAll('img[data-bs-toggle="modal"]');
    const moreButton = document.querySelector('button[data-bs-toggle="modal"]');
    const carouselInner = document.querySelector('.carousel-inner');

    const clearActiveClasses = () => {
        carouselInner.querySelectorAll('.carousel-item').forEach(item => item.classList.remove('active'));
    };

    const setActiveImage = (index) => {
        clearActiveClasses();
        carouselInner.querySelector(`.carousel-item:nth-child(${index + 1})`).classList.add('active');
    };

    imageElements.forEach(img => {
        img.addEventListener('click', () => setActiveImage(parseInt(img.getAttribute('data-index'), 10)));
    });

    moreButton.addEventListener('click', () => setActiveImage(parseInt(moreButton.getAttribute('data-index'), 10)));
});


//公告
document.addEventListener("DOMContentLoaded", function () {
    const content = document.querySelector('.extra-content');
    const button = document.getElementById('toggleButton');

    button.addEventListener("click", function () {
        if (content.style.display === 'none') {
            content.style.display = 'block';
            button.textContent = button.getAttribute('data-less'); // 使用 data-less 屬性
        } else {
            content.style.display = 'none';
            button.textContent = button.getAttribute('data-more'); // 使用 data-more 屬性
        }
    });

    // 確保初始狀態設置為隱藏
    content.style.display = 'none';
});

//評論區
$(document).on("click", ".page-link", function (e) {
    e.preventDefault();
    var page = $(this).data("page");
    var restaurantId = $("#reviews-container").data("restaurant-id");

    $.ajax({
        url: '/Restaurant/GetReviews',
        type: 'GET',
        data: { id: restaurantId, page: page },
        success: function (data) {
            $("#reviews-container").html(data);
        }
    });
});

//收藏
document.querySelectorAll('.favorite').forEach(button => {
    button.addEventListener('click', function () {
        const restaurantId = this.dataset.restaurantId;

        fetch('/Restaurant/ToggleFavorite', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(restaurantId)
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    // 根據收藏狀態切換按鈕樣式
                    if (data.isFavorite) {
                        this.classList.add('favorite_active');
                        this.querySelector('i').classList.add('bi-heart-fill');
                        this.querySelector('i').classList.remove('bi-heart');
                        this.querySelector('.favorite-text').textContent = '已收藏';  // 更新文字為 "已收藏"
                    } else {
                        this.classList.remove('favorite_active');
                        this.querySelector('i').classList.remove('bi-heart-fill');
                        this.querySelector('i').classList.add('bi-heart');
                        this.querySelector('.favorite-text').textContent = '收藏';  // 更新文字為 "收藏"
                    }
                } else {
                    alert(data.message);  // 顯示錯誤訊息
                }
            })
            .catch(error => console.error('Error:', error));
    });
});

//訂位
document.getElementById('bookingLink').addEventListener('click', function (event) {
    event.preventDefault();
    // 阻止默認行為
    // 跳轉到指定頁面，傳遞參數 
    window.location.href = '@Url.Action("BookingPage", "Booking", new { RestaurantId = Model.Restaurant.RestaurantId })';
});



