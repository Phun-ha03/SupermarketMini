var connect_error_message = "Lỗi kết nối, xin vui lòng thử lại sau!";
var error_title = "Lỗi";
var warning_title = "Cảnh báo";
var confirm_title = "Xác nhận";
var message_title = "Thông báo";
var api_success_status = true;
$(document).ready(function () {
  /*$(".table-responsive").on("show.bs.dropdown", function () {
    $(".table-responsive").css("overflow", "inherit");
  });

  $(".table-responsive").on("hide.bs.dropdown", function () {
    $(".table-responsive").css("overflow", "auto");
  });*/

  $(".select2").select2({ width: "100%" });

  $(".formattedIntField").on("keyup", function () {
    var s = $(this).val();
    if (s != null && s.length > 0) {
      s = s.replace(/\D/g, "");
      if (s.length > 0) {
        var n = parseInt(s, 10);
        s = n.toLocaleString("en-US");
      }
      $(this).val(s.replaceAll(",", "."));
    }
  });

  $(".formattedDoubleField").on("keyup", function () {
    var s = $(this)
      .val()
      .replace(/[^\d.]/g, "");
    if (s.length <= 0) {
      s = "";
    } else {
      if (s.indexOf(".") > 0) {
        var s_split = s.split(".");
        s =
          parseInt(
            s_split[0].length <= 0 ? "0" : s_split[0],
            10
          ).toLocaleString("en-US") +
          "." +
          s_split[1];
      } else {
        s = parseInt(s, 10).toLocaleString("en-US");
      }
    }
    $(this).val(s);
    //n.toLocaleString('en-US', {minimumFractionDigits: 0, maximumFractionDigits: 2})
  });

  $(".localNumberInput").on("keyup", function () {
    var s = $(this)
      .val()
      .replace(/[^\d,]/g, "");
    if (s.length <= 0) {
      s = "";
    } else {
      if (s.indexOf(",") > 0) {
        var s_split = s.split(",");
        s =
          parseInt(
            s_split[0].length <= 0 ? "0" : s_split[0],
            10
          ).toLocaleString("vi-VN") +
          "," +
          s_split[1];
      } else {
        s = parseInt(s, 10).toLocaleString("vi-VN");
      }
    }
    $(this).val(s);
  });
});

function setDatepicker(id) {
  flatpickr("#" + id, {
    enableTime: true,
    enableSeconds: true,
    time_24hr: true,
    dateFormat: "d/m/Y H:i:S",
    allowInput: true,
    autoclose: true,
    clickOpens: false,
    position: "above",
  }).open();
}

function setDateOnlyPicker(id) {
  flatpickr("#" + id, {
    enableTime: false,
    enableSeconds: true,
    time_24hr: true,
    dateFormat: "d/m/Y",
    allowInput: true,
    autoclose: true,
    clickOpens: false,
    position: "above",
  }).open();
}

function onCancel_Click(reload = false) {
    try {
        window.parent.closeRemoteModal(reload);
    }
    catch (e) { }
    try {
        window.parent.closeRemoteModalNotRefresh(); 
    }
    catch (e) { }
  return false;
}

function onDone() {
  window.parent.closeRemoteModal(true);
  return false;
}

function openVersionModal() {
  $("#versionModel").modal({ backdrop: "static", keyboard: false });
  $("#versionModel").modal("show");
}
function closeVersionModal() {
  $("#versionModel").modal("hide");
}
function openLogoutModal() {
  $("#logoutModel").modal({ backdrop: "static", keyboard: false });
  $("#logoutModel").modal("show");
}
function closeLogoutModal() {
  $("#logoutModel").modal("hide");
}
function openAlertModal(title, mess) {
  $("#alertTitle").text(title);
  $("#alertMessage").text(mess);
  $("#alertModel").modal({ backdrop: "static", keyboard: false });
  $("#alertModel").modal("show");
}
function closeAlertModal() {
  $("#alertTitle").text("");
  $("#alertMessage").text("");
  $("#alertModel").modal("hide");
}
function openConfirmModal(btn, mess, width = null, height = null) {
  $("#confirmModelContent").html("");
  if (width != null) {
    if (width.indexOf("%") >= 0) {
      let widthInt = screen.width;
      var percent = parseInt(width.replace("%", ""));
      width = (widthInt * percent) / 100 + "px";
    }
    //$('#confirmModelContainer').css("max-width", width);
    $("#confirmModelContainer").css("width", width);
  }
  if (height != null) {
    if (height.indexOf("%") >= 0) {
      let heightInt = screen.height - 150;
      var percent = parseInt(height.replace("%", ""));
      height = (heightInt * percent) / 100 + "px";
    }
    $("#confirmModelContainer").css("max-height", height);
    $("#confirmModelContainer").css("height", height);
  }
  //console.log(mess);
  $("#confirmModelButton").text(btn);
  $("#confirmModelContent").html(mess);
  $("#confirmModel").modal({ backdrop: "static", keyboard: false });
  $("#confirmModel").modal("show");
}
function closeConfirmModal() {
  $("#confirmModelButton").text("");
  $("#confirmMessage").html("");
  $("#confirmModel").modal("hide");
}
function resetConfirmButton() {
  $("#confirmModelButton").off("click");
  return true;
}
function resetConfirmCancelButton() {
    $("#confirmModelCancelButton").off("click");
    return true;
}

function logout() {
  window.location = "/authen/logout";
}

function openRemoteModal(page, title, width = null, height = null) {
  if (width != null) {
    if (width.indexOf("%") >= 0) {
      let widthInt = screen.width;
      var percent = parseInt(width.replace("%", ""));
      width = (widthInt * percent) / 100 + "px";
    }
    //$('#remoteModelContainer').css("max-width", width);
    $("#remoteModelContainer").css("width", width);
  }
  if (height != null) {
    if (height.indexOf("%") >= 0) {
      let heightInt = screen.height - 150;
      var percent = parseInt(height.replace("%", ""));
      height = (heightInt * percent) / 100 + "px";
    }
    $("#remoteModelContainer").css("max-height", height);
    $("#remoteModelContainer").css("height", height);
  }

  $("#remoteModelContent").html("");
  $("#remoteModelTitle").text(title);
  $("#remoteModelContent").html(
    '<iframe style="border: 0px; margin:0px; padding:0px;" src="' +
      page +
      '" width=100%" height="' +
      height +
      '"></iframe>'
  );

  $("#remoteModel").modal({ backdrop: "static", keyboard: false });
  $("#remoteModel").modal("show");
}
function closeRemoteModal(reload) {
  $("#remoteModelTitle").text("");
  $("#remoteModelContent").html("");
  $("#remoteModel").modal("hide");
  if (reload) {
    window.location.reload();
  }
}
function openRemoteModalNotRefresh(page, title, width = null, height = null, removeClosebtn=false) {
    if (width != null) {
        if (width.indexOf("%") >= 0) {
            let widthInt = screen.width;
            var percent = parseInt(width.replace("%", ""));
            width = (widthInt * percent) / 100 + "px";
        }
        //$('#remoteModelContainer').css("max-width", width);
        $("#remoteModelNotRefreshContainer").css("width", width);
        if (removeClosebtn) {
            $('.btn-close').remove()
        }
    }
    if (height != null) {
        if (height.indexOf("%") >= 0) {
            let heightInt = screen.height - 150;
            var percent = parseInt(height.replace("%", ""));
            height = (heightInt * percent) / 100 + "px";
        }
        $("#remoteModelNotRefreshContainer").css("max-height", height);
        $("#remoteModelNotRefreshContainer").css("height", height);
    }

    $("#remoteModelNotRefreshContent").html("");
    $("#remoteModelNotRefreshTitle").text(title);
    $("#remoteModelNotRefreshContent").html(
        '<iframe style="border: 0px; margin:0px; padding:0px;" src="' +
        page +
        '" width=100%" height="' +
        height +
        '"></iframe>'
    );

    $("#remoteModelNotRefresh").modal({ backdrop: "static", keyboard: false });
    $("#remoteModelNotRefresh").modal("show");
}
function closeRemoteModalNotRefresh() {
    $("#remoteModelNotRefreshTitle").text("");
    $("#remoteModelNotRefreshContent").html("");
    $("#remoteModelNotRefresh").modal("hide");
}
function openRemoteModalContentSplitting(
  page,
  title,
  width = null,
  height = null
) {
  if (width != null) {
    if (width.indexOf("%") >= 0) {
      let widthInt = screen.width;
      var percent = parseInt(width.replace("%", ""));
      width = (widthInt * percent) / 100 + "px";
    }
    //$('#remoteModelContainer').css("max-width", width);
    $("#remoteModelContentSplittingContainer").css("width", width);
  }
  if (height != null) {
    if (height.indexOf("%") >= 0) {
      let heightInt = screen.height - 150;
      var percent = parseInt(height.replace("%", ""));
      height = (heightInt * percent) / 100 + "px";
    }
    $("#remoteModelContentSplittingContainer").css("max-height", height);
    $("#remoteModelContentSplittingContainer").css("height", height);
  }

  $("#remoteModelContentSplittingContent").html("");
  $("#remoteModelContentSplittingTitle").text(title);
  $("#remoteModelContentSplittingContent").html(
    '<iframe style="border: 0px; margin:0px; padding:0px;" src="' +
      page +
      '" width=100%" height="' +
      height +
      '"></iframe>'
  );

  $("#remoteModelContentSplitting").modal({
    backdrop: "static",
    keyboard: false,
  });
  $("#remoteModelContentSplitting").modal("show");
}
function closeRemoteModalContentSplitting(reload) {
  /*resetConfirmButton();
    $("#confirmModelButton").on("click", {}, function () {
        resetConfirmButton();
        closeConfirmModal();
    });
    openConfirmModal('Đồng ý', "Bạn muốn đóng cửa sổ tách văn bản? </br><b>Lưu ý:</b> Lưu dữ liệu trước khi đóng.");
    */

  $("#remoteModelContentSplittingTitle").text("");
  $("#remoteModelContentSplittingContent").html("");
  $("#remoteModelContentSplitting").modal("hide");
  if (reload) {
    window.location.reload();
  }
}
function closeDialog(reload) {
  $("#remoteModelTitle").text("");
  $("#remoteModelContent").html("");
  $("#remoteModel").modal("hide");
  if (reload) {
    window.location.reload();
  }
}
function onUpdateSuccess() {
  closeRemoteModal(true);
  return false;
}
function onDeleteSuccess(ids) {
  closeRemoteModal(false);
  var idsArr = ids.split(",");
  for (var i = 0; i < idsArr.length; i++) {
    $("#" + idsArr[0]).remove();
  }
  return false;
}

function onQuickLoginSuccess() {
  closeRemoteModal(false);
  return false;
}

function checkSession() {
  var response = $.ajax({
    type: "GET",
    url: "/PublicApi/CheckSession",
    contentType: false,
    cache: false,
    processData: false,
    async: false,
  });
  if (response != null && response.readyState == 4) {
    data = response.responseJSON;
    if (data != null && data.Status == 1) {
      return true;
    }
  }
  openRemoteModal("/Authen/QuickLogin", "Đăng nhập lại", "400px", "160px");
  return false;
}

function btnCollapseClick(id) {
  var btnCollapse = $("#btn_collapse_" + id);
  var action = "";
  if (btnCollapse.hasClass("fa-angle-down")) {
    btnCollapse.removeClass("fa-angle-down");
    btnCollapse.addClass("fa-angle-right");
    action = "collapse";
  } else {
    btnCollapse.removeClass("fa-angle-right");
    btnCollapse.addClass("fa-angle-down");
    action = "expand";
  }
  var subElements = [];
  $(".module_item[parent-id='" + id + "']").each(function () {
    var id2 = $(this).attr("id");
    subElements.push(id2);

    $(".module_item[parent-id='" + id2 + "']").each(function () {
      var id3 = $(this).attr("id");
      subElements.push(id3);

      $(".module_item[parent-id='" + id3 + "']").each(function () {
        var id4 = $(this).attr("id");
        subElements.push(id4);

        $(".module_item[parent-id='" + id4 + "']").each(function () {
          var id5 = $(this).attr("id");
          subElements.push(id5);

          $(".module_item[parent-id='" + id5 + "']").each(function () {
            var id6 = $(this).attr("id");
            subElements.push(id6);
          });
        });
      });
    });
  });
  for (var i = 0; i < subElements.length; i++) {
    if (action == "collapse") {
      if (!$("#" + subElements[i]).hasClass("d-none")) {
        $("#" + subElements[i]).addClass("d-none");
        $("#" + subElements[i]).attr("toogle-collapse-by", id);
      }
    } else {
      if (
        $("#" + subElements[i]).hasClass("d-none") &&
        $("#" + subElements[i]).attr("toogle-collapse-by") == id
      ) {
        $("#" + subElements[i]).removeClass("d-none");
        $("#" + subElements[i]).attr("toogle-collapse-by", "");
      }
    }
  }
  return false;
}

function genGuid() {
  return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function (c) {
    var r = (Math.random() * 16) | 0,
      v = c === "x" ? r : (r & 0x3) | 0x8;
    return v.toString(16);
  });
}

function updateDateValie(element, value) {
  if (value == null || value.length < 0) {
    $(element).val("");
  } else {
    var dateSplit = value.split("/");
    $(element).val(dateSplit[1] + "/" + dateSplit[0] + "/" + dateSplit[2]);
  }
}

function updateDateTimeValie(element, value) {
  if (value == null || value.length < 0) {
    $(element).val("");
  } else {
    var datetimeSplit = value.split(" ");

    var dateSplit = datetimeSplit[0].split("/");
    var timeSplit = datetimeSplit[1].split(":");

    var newDateTimeStr = dateSplit[1] + "/" + dateSplit[0] + "/" + dateSplit[2];
    newDateTimeStr += " " + datetimeSplit[1];

    if (timeSplit.length == 2) newDateTimeStr += ":00";

    $(element).val(newDateTimeStr);
  }
}

function removeSignature(str) {
  str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
  str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
  str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
  str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
  str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
  str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
  str = str.replace(/đ/g, "d");
  str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "A");
  str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "E");
  str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "I");
  str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, "O");
  str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, "U");
  str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, "Y");
  str = str.replace(/Đ/g, "D");
  // Some system encode vietnamese combining accent as individual utf-8 characters
  // Một vài bộ encode coi các dấu mũ, dấu chữ như một kí tự riêng biệt nên thêm hai dòng này
  str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, ""); // ̀ ́ ̃ ̉ ̣  huyền, sắc, ngã, hỏi, nặng
  str = str.replace(/\u02C6|\u0306|\u031B/g, ""); // ˆ ̆ ̛  Â, Ê, Ă, Ơ, Ư
  // Remove extra spaces
  // Bỏ các khoảng trắng liền nhau
  str = str.replace(/ + /g, " ");
  str = str.trim();
  // Remove punctuations
  // Bỏ dấu câu, kí tự đặc biệt
  str = str.replace(
    /!|@@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|\"|\&|\#|\[|\]|~|\$|_|`|-|{|}|\||\\/g,
    " "
  );
  return str;
}

function makeToast(type, message) {
  var bg = "";
  if (type.toLowerCase() == "error") {
    bg = "danger";
  } else if (type.toLowerCase() == "warning") {
    bg = "warning";
  } else if (type.toLowerCase() == "info") {
    bg = "info";
  } else {
    bg = "success";
  }

  $("#toast").addClass("bg-" + bg);
  $("#toast-body").html(message);
  $("#toast").show("slow", "swing");

  setTimeout(() => {
    $("#toast").removeClass("bg-" + bg);
    $("#toast-body").text("");
    $("#toast").hide("slow", "swing");
  }, 5000);
}
function makeUpdatedHighlight(id) {
  if (id.indexOf("#") != 0) id = "#" + id;

  $(id).addClass("updated-row-bg");

  setTimeout(() => {
    $(id).removeClass("updated-row-bg");
  }, 5000);
}

var selectedTableRowIds = [];
function onTableRowSelect(tbody, row_id, check_prefix = "chkSelect_") {
  if (window.event.ctrlKey) {
    if (selectedTableRowIds.includes(row_id)) {
      newItems = [];
      for (var i = 0; i < selectedTableRowIds.length; i++) {
        if (selectedTableRowIds[i] != row_id) {
          newItems.push(selectedTableRowIds[i]);
          if (
            $("#" + selectedTableRowIds[i]).hasClass("selected-row-bg") == false
          ) {
            $("#" + selectedTableRowIds[i]).addClass("selected-row-bg");
            if ($("#" + check_prefix + selectedTableRowIds[i]).length > 0) {
              $("#" + check_prefix + selectedTableRowIds[i]).prop(
                "checked",
                true
              );
            }
          }
        } else {
          $("#" + selectedTableRowIds[i]).removeClass("selected-row-bg");
          if ($("#" + check_prefix + selectedTableRowIds[i]).length > 0) {
            $("#" + check_prefix + selectedTableRowIds[i]).prop(
              "checked",
              false
            );
          }
        }
      }
      selectedTableRowIds = newItems;
    } else {
      selectedTableRowIds.push(row_id);
      $("#" + row_id).addClass("selected-row-bg");
      if ($("#" + check_prefix + row_id).length > 0) {
        $("#" + check_prefix + row_id).prop("checked", true);
      }
    }
  } else if (window.event.shiftKey && selectedTableRowIds.length > 0) {
    var curr_index = parseInt($("#Index_" + row_id).text());
    var start_index = parseInt($("#Index_" + selectedTableRowIds[0]).text());
    var min = start_index < curr_index ? start_index : curr_index;
    var max = start_index < curr_index ? curr_index : start_index;

    selectedTableRowIds = [];
    $("#" + tbody + ">tr").each(function (index) {
      var id = $(this).attr("id");
      var index = parseInt($("#Index_" + id).text());
      if (index >= min && index <= max) {
        if (curr_index > start_index) {
          selectedTableRowIds.push(id);
        } else {
          selectedTableRowIds.unshift(id);
        }
        if ($(this).hasClass("selected-row-bg") == false) {
          $(this).addClass("selected-row-bg");
          if ($("#" + check_prefix + id).length > 0) {
            $("#" + check_prefix + id).prop("checked", true);
          }
        }
      } else {
        $(this).removeClass("selected-row-bg");
        if ($("#" + check_prefix + id).length > 0) {
          $("#" + check_prefix + id).prop("checked", false);
        }
      }
    });
  } else {
    selectedTableRowIds = [];
    $("#" + tbody + ">tr.selected-row-bg").each(function (index) {
      var id = $(this).attr("id");
      $(this).removeClass("selected-row-bg");
      if ($("#" + check_prefix + id).length > 0) {
        $("#" + check_prefix + id).prop("checked", false);
      }
    });

    selectedTableRowIds.push(row_id);
    $("#" + row_id).addClass("selected-row-bg");
    if ($("#" + check_prefix + row_id).length > 0) {
      $("#" + check_prefix + row_id).prop("checked", true);
    }
  }
}

function onTableRowSelectSingle(tbody, row_id, check_prefix = "chkSelect_") {
  selectedTableRowIds = [];
  selectedTableRowIds.push(row_id);

  $("#" + tbody + ">tr.selected-row-bg").each(function (index) {
    var id = $(this).attr("id");
    $(this).removeClass("selected-row-bg");
    if ($("#" + check_prefix + id).length > 0) {
      $("#" + check_prefix + id).prop("checked", false);
    }
  });

  $("#" + row_id).addClass("selected-row-bg");
  if ($("#" + check_prefix + row_id).length > 0) {
    $("#" + check_prefix + row_id).prop("checked", true);
  }
}

function chkSelect_OnChange(id, check_prefix = "chkSelect_") {
  var checked = $("#" + check_prefix + id).prop("checked");
  if (checked) {
    if ($("#" + id).length > 0) {
      $("#" + id).addClass("selected-row-bg");
    }
    if (
      selectedTableRowIds.length <= 0 ||
      !arrayContains(selectedTableRowIds, id)
    ) {
      selectedTableRowIds.push(id);
    }
  } else {
    if ($("#" + id).length > 0) {
      $("#" + id).removeClass("selected-row-bg");
    }
    if (
      selectedTableRowIds.length > 0 &&
      arrayContains(selectedTableRowIds, id)
    ) {
      selectedTableRowIds.unshift(id);
    }
  }
}

function arrayContains(arr, obj) {
  var retVal = false;
  if (arr != null && arr.length > 0) {
    for (var i = 0; i < arr.length; i++) {
      if (arr[i] == obj) {
        retVal = true;
        break;
      }
    }
  }
  return retVal;
}

function onSelectAllRowChange(
  chk_all_id = "chkSelectAll",
  chk_class = "check-item"
) {
  var checked = $("#" + chk_all_id).is(":checked");
  $("." + chk_class).each(function (index) {
    var id = $(this).attr("data-id");
    $(this).prop("checked", checked);
    if (checked) {
      if ($("#" + id).length > 0) {
        $("#" + id).addClass("selected-row-bg");
      }
      if (
        selectedTableRowIds.length <= 0 ||
        !arrayContains(selectedTableRowIds, id)
      ) {
        selectedTableRowIds.push(id);
      }
    } else {
      if ($("#" + id).length > 0) {
        $("#" + id).removeClass("selected-row-bg");
      }
      if (
        selectedTableRowIds.length > 0 &&
        arrayContains(selectedTableRowIds, id)
      ) {
        selectedTableRowIds.unshift(id);
      }
    }
  });
}

function getDate(dateStr) {
  if (typeof dateStr === "undefined" || dateStr == null) {
    return null;
  }

  return new Date(dateStr);
}

function dateToString(dateStr) {
  if (typeof dateStr === "undefined" || dateStr == null) {
    return "";
  }

  var retVal = "";

  var date = null;

  if (dateStr instanceof Date) {
    date = dateStr;
  } else {
    date = new Date(dateStr);
  }

  if (date == null) return "";

  retVal =
    (date.getDate() < 10 ? "0" : "") +
    date.getDate() +
    "/" +
    (date.getMonth() + 1 < 10 ? "0" : "") +
    (date.getMonth() + 1) +
    "/" +
    date.getFullYear();

  return retVal;
}

function displayString(str) {
  if (typeof str === "undefined" || str == null) {
    return "";
  }
  return str;
}

function intToString(intStr) {
  if (typeof intStr === "undefined" || intStr == null) {
    return "0";
  }
  return parseInt(intStr).toLocaleString("en-US");
}

function doubleToString(doubleStr) {
  if (typeof doubleStr === "undefined" || doubleStr == null) {
    return "0";
  }
  return parseFloat(doubleStr).toLocaleString("en-US");
}

function stringToInt(str) {
  if (typeof str === "undefined" || str == null) {
    return 0;
  }
  return parseInt(str.replaceAll(",", ""));
}

function stringToFloat(str) {
  if (typeof str === "undefined" || str == null) {
    return 0;
  }
  return parseFloat(str.replaceAll(",", ""));
}

function getNextRowIndexOfTable(tableId) {
  if (typeof tableId === "undefined" || tableId == null) {
    return 0;
  }
  return $("#" + table + ">tr").length + 1;
}

function take_decimal_number(num, n) {
  //num : số cần xử lý
  //n: số chữ số sau dấu phẩy cần lấy
  let base = 10 ** n;
  let result = Math.round(num * base) / base;
  return result;
}

function readInt(el) {
  if (el.val() == null || el.val().trim().length <= 0) return 0;
  return parseInt(el.val().trim().replaceAll(".", "").replaceAll(",", ""));
}

function readFloat(el) {
  if (el.val() == null || el.val().trim().length <= 0) return 0;
  return parseFloat(el.val().trim().replaceAll(".", "").replaceAll(",", "."));
}

function readString(el) {
  if (el.val() == null) return "";
  return el.val().trim();
}

function readDate(el) {
  if (el.val() == null || el.val().trim().length <= 0) return null;
  return new Date(el.val().trim());
}

function readIntFromLabel(el) {
  if (el.text() == null || el.text().trim().length <= 0) return 0;
  return parseInt(el.text().trim().replaceAll(".", "").replaceAll(",", ""));
}

function readFloatFromLabel(el) {
  if (el.text() == null || el.text().trim().length <= 0) return 0;
  return parseFloat(el.text().trim().replaceAll(".", "").replaceAll(",", "."));
}

function readStringFromLabel(el) {
  if (el.text() == null) return "";
  return el.text().trim();
}

function readDateFromLabel(el) {
  if (el.text() == null || el.text().trim().length <= 0) return null;
  return new Date(el.text().trim());
}

function getCheckboxValue(el) {
  return $(el).is(":checked");
}

function setCheckboxValue(el, val) {
  $(el).prop("checked", val);
}

function getRadioGroupValue(name) {
  return $('input[name="' + name + '"]:checked').val();
}

function getRadioValue(el) {
  return $(el).is(":checked");
}

function setRadioValue(el, val) {
  $(el).prop("checked", val);
}

function MMddyyyyHHmmssToDate(str) {
  if (typeof str === "undefined") return null;
  if (str == null || str.length <= 0) return null;

  var day = 0,
    month = 0;
  (year = 0), (hours = 0), (mins = 0), (seconds = 0);

  str = str.replaceAll("-", "/");
  var strSplit = str.split(" ");
  if (strSplit.length != 2) return null;

  var date = strSplit[0].trim();
  var time = strSplit[1].trim();

  var dateSplit = date.split("/");
  if (dateSplit.length != 3) {
    return null;
  }
  month = parseInt(dateSplit[0]);
  day = parseInt(dateSplit[1]);
  year = parseInt(dateSplit[2]);

  var timeSplit = time.split(":");
  if (timeSplit.length == 2) {
    timeSplit[2] = "00";
  }
  if (dateSplit.length != 3) {
    return null;
  }
  hours = parseInt(timeSplit[0]);
  mins = parseInt(timeSplit[1]);
  seconds = parseInt(timeSplit[2]);

  return new Date(year, month - 1, day, hours, mins, seconds);
}

function ddMMyyyyHHmmssToDate(str) {
  if (typeof str === "undefined") return null;
  if (str == null || str.length <= 0) return null;

  var day = 0,
    month = 0;
  (year = 0), (hours = 0), (mins = 0), (seconds = 0);

  str = str.replaceAll("-", "/");
  var strSplit = str.split(" ");
  if (strSplit.length != 2) return null;

  var date = strSplit[0].trim();
  var time = strSplit[1].trim();

  var dateSplit = date.split("/");
  if (dateSplit.length != 3) {
    return null;
  }
  day = parseInt(dateSplit[0]);
  month = parseInt(dateSplit[1]);
  year = parseInt(dateSplit[2]);

  var timeSplit = time.split(":");
  if (timeSplit.length == 2) {
    timeSplit[2] = "00";
  }
  if (dateSplit.length != 3) {
    return null;
  }
  hours = parseInt(timeSplit[0]);
  mins = parseInt(timeSplit[1]);
  seconds = parseInt(timeSplit[2]);

  return new Date(year, month - 1, day, hours, mins, seconds);
}

function MMddyyyyToDate(str) {
  if (typeof str === "undefined") return null;
  if (str == null || str.length <= 0) return null;

  var day = 0,
    month = 0;
  (year = 0), (hours = 0), (mins = 0), (seconds = 0);

  str = str.replaceAll("-", "/");

  var dateSplit = str.split("/");
  if (dateSplit.length != 3) {
    return null;
  }
  month = parseInt(dateSplit[0]);
  day = parseInt(dateSplit[1]);
  year = parseInt(dateSplit[2]);

  return new Date(year, month - 1, day, hours, mins, seconds);
}

function ddMMyyyyToDate(str) {
  if (typeof str === "undefined") return null;
  if (str == null || str.length <= 0) return null;

  var day = 0,
    month = 0;
  (year = 0), (hours = 0), (mins = 0), (seconds = 0);

  str = str.replaceAll("-", "/");

  var dateSplit = str.split("/");
  if (dateSplit.length != 3) {
    return null;
  }
  day = parseInt(dateSplit[0]);
  month = parseInt(dateSplit[1]);
  year = parseInt(dateSplit[2]);

  return new Date(year, month - 1, day, hours, mins, seconds);
}

function dateToddMMyyyy(dateStr) {
  if (typeof dateStr === "undefined" || dateStr == null) {
    return "";
  }

  var retVal = "";

  var date = null;

  if (dateStr instanceof Date) {
    date = dateStr;
  } else {
    date = new Date(dateStr);
  }

  if (date == null) return "";

  retVal =
    (date.getDate() < 10 ? "0" : "") +
    date.getDate() +
    "/" +
    (date.getMonth() + 1 < 10 ? "0" : "") +
    (date.getMonth() + 1) +
    "/" +
    date.getFullYear();

  return retVal;
}

function dateToddMMyyyyHHmmss(dateStr) {
  if (typeof dateStr === "undefined" || dateStr == null) {
    return "";
  }

  var retVal = "";

  var date = null;

  if (dateStr instanceof Date) {
    date = dateStr;
  } else {
    date = new Date(dateStr);
  }

  if (date == null) return "";

  retVal =
    (date.getDate() < 10 ? "0" : "") +
    date.getDate() +
    "/" +
    (date.getMonth() + 1 < 10 ? "0" : "") +
    (date.getMonth() + 1) +
    "/" +
    date.getFullYear() +
    " " +
    (date.getHours() < 10 ? "0" : "") +
    date.getHours() +
    ":" +
    (date.getMinutes() < 10 ? "0" : "") +
    date.getMinutes() +
    ":" +
    (date.getSeconds() < 10 ? "0" : "") +
    date.getSeconds();

  return retVal;
}

function dateToMMddyyyy(dateStr) {
  if (typeof dateStr === "undefined" || dateStr == null) {
    return "";
  }

  var retVal = "";

  var date = null;

  if (dateStr instanceof Date) {
    date = dateStr;
  } else {
    date = new Date(dateStr);
  }

  if (date == null) return "";

  retVal =
    (date.getMonth() + 1 < 10 ? "0" : "") +
    (date.getMonth() + 1) +
    "/" +
    (date.getDate() < 10 ? "0" : "") +
    date.getDate() +
    "/" +
    date.getFullYear();

  return retVal;
}

function dateToMMddyyyyHHmmss(dateStr) {
  if (
    typeof dateStr === "undefined" ||
    dateStr == null ||
    (!(dateStr instanceof Date) && dateStr.trim().length <= 0)
  ) {
    return "";
  }

  var retVal = "";

  var date = null;

  if (dateStr instanceof Date) {
    date = dateStr;
  } else {
    date = new Date(dateStr);
  }

  if (date == null) return "";

  retVal =
    (date.getMonth() + 1 < 10 ? "0" : "") +
    (date.getMonth() + 1) +
    "/" +
    (date.getDate() < 10 ? "0" : "") +
    date.getDate() +
    "/" +
    date.getFullYear() +
    " " +
    (date.getHours() < 10 ? "0" : "") +
    date.getHours() +
    ":" +
    (date.getMinutes() < 10 ? "0" : "") +
    date.getMinutes() +
    ":" +
    (date.getSeconds() < 10 ? "0" : "") +
    date.getSeconds();

  return retVal;
}

function dateToyyyyMMddHHmmss(dateStr) {
  if (
    typeof dateStr === "undefined" ||
    dateStr == null ||
    (!(dateStr instanceof Date) && dateStr.trim().length <= 0)
  ) {
    return "";
  }

  var retVal = "";

  var date = null;

  if (dateStr instanceof Date) {
    date = dateStr;
  } else {
    date = new Date(dateStr);
  }

  if (date == null) return "";

  retVal =
    date.getFullYear() +
    "/" +
    (date.getMonth() + 1 < 10 ? "0" : "") +
    (date.getMonth() + 1) +
    "/" +
    (date.getDate() < 10 ? "0" : "") +
    date.getDate() +
    " " +
    (date.getHours() < 10 ? "0" : "") +
    date.getHours() +
    ":" +
    (date.getMinutes() < 10 ? "0" : "") +
    date.getMinutes() +
    ":" +
    (date.getSeconds() < 10 ? "0" : "") +
    date.getSeconds();

  return retVal;
}

function readBool(el) {
  if (
    el.val() != null ||
    el.val().length > 0 ||
    el.val().toLowerCase() == "true"
  )
    return true;
  return false;
}

function stringIsNullOrEmpty(str) {
  if (str == null || str.length <= 0 || str.toLowerCase() == "null")
    return true;
  return false;
}

function javaDateToCDateString(jdate) {
  //return (new Date(jdate.getTime() - jdate.getTimezoneOffset() * 60000)).toJSON()
  return dateToyyyyMMddHHmmss(jdate);
}

function getFullnamePrefix(name) {
  if (name == null || name.length <= 0) return "";
  if (name.length <= 2) return name.toUpperCase();
  if (name.indexOf(" ") < 0) return name.substring(0, 2).toUpperCase();
  var nameSplit = name.split(" ");
  return (
    nameSplit[0].substring(0, 1) +
    nameSplit[nameSplit.length - 1].substring(0, 1)
  ).toUpperCase();
}

function getUrlParameter(sParam) {
  var sPageURL = window.location.search.substring(1),
    sURLVariables = sPageURL.split("&"),
    sParameterName,
    i;

  for (i = 0; i < sURLVariables.length; i++) {
    sParameterName = sURLVariables[i].split("=");

    if (sParameterName[0] === sParam) {
      return sParameterName[1] === undefined
        ? true
        : decodeURIComponent(sParameterName[1]);
    }
  }
  return false;
}
