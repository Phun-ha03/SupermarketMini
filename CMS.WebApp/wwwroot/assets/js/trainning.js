var searchContent = "";
function renderKatex(ms) {
  var contentRenderMs = "";
  var reg = /\\[\[\]()]+/;
  function replaceText(text) {
    const regex = /([\w\d\s-]+)\(([^)]+)\)/g;
    const replacedText = text.replace(regex, (insideText) => {
      return `\\text{${insideText}}`;
    });
    return replacedText;
  }
  ms.split(reg).forEach((x, i) => {
    var contentR =
      i % 2 != 0
        ? katex.renderToString(replaceText(x), {
            displayMode: x.includes("\n") == true ? true : false,
            leqno: true,
            fleqn: true,
            throwOnError: true,
            errorColor: "#cc0000",
            strict: "ignore",
            output: "html",
            trust: true,
          })
        : x;
    contentRenderMs += contentR;
  });
  return contentRenderMs;
}

function strDoc(str) {
  const regex = /\[([^\]]+)\]\((()[^\)]+)\)/g;
  const newText = str.replace(regex, (match, p1, p2) => {
    const regexRemoveDomain =
      /(http|https:\/\/)?\/(DocChunkView|DocItemView)\/\d+/g;
    const isDocChunkView =
      p2.toUpperCase().includes("docchunkview".toUpperCase()) == true ||
      p2.toUpperCase().includes("docitemview".toUpperCase()) == true;
    var returnString =
      isDocChunkView == true
        ? `<a class='doc-link tip-bottom' data-tool="Click để xem nội dung" href="${p2.match(
            regexRemoveDomain
          )}">${p1}</a>`
        : match;
    return returnString;
  });
  return newText;
}
function replaceText(docx) {
  return displayString(
    docx["leadHtml"] ??
      docx["docItemLeadFullHtml"] ??
      docx["docItemParentLeadHtml"]
  )
    .replaceAll("\r\n", "<br/>")
    .replaceAll("\\r\\n", "<br/>");
}
function renderDocName(docx) {
  return `<md-block>[${displayString(
    docx["docName"]
  )}](https://luatvietnam.vn/${docx["docUrl"]})</md-block>`;
}
$(document).on("click", ".doc-link", function (event) {
  event.preventDefault();
  openDialogDoc($(this).attr("href"));
});
function openDialogDoc(doclink) {
  if (doclink !== null || doclink !== "" || doclink !== undefined) {
    var data = doclink.split("/");
    var request = {
      isDocChunk:
        data[0].toLowerCase().includes("DocChunk".toLowerCase()) ||
        data[1].toLowerCase().includes("DocChunk".toLowerCase())
          ? true
          : false,
      isDocItem:
        data[0].toLowerCase().includes("DocItem".toLowerCase()) ||
        data[1].toLowerCase().includes("DocItem".toLowerCase())
          ? true
          : false,
      id:
        data[0] == "" || data[0] == null || data[0] == undefined
          ? data[2]
          : data[1],
    };
    $.ajax({
      type: "POST",
      url: "/CMSApi/GetDocChunkOrDocItem",
      dataType: "json",
      contentType: "application/json;charset=utf-8",
      data: JSON.stringify(request),
      success: function (result) {
        if (result.isSuccessed) {
          var doc = result.resultObj;
          var content =
            "<div class='modal fade show' id='remoteModelNotRefresh3' tabindex='-1' aria-modal='true' role='dialog' style='display: block;'>" +
            "         <div class='modal-dialog modal-dialog-centered' role='dialog' id='remoteModelNotRefreshContainer2' style='max-width: 90% !important;z-index:9999 !important' >" +
            "             <div class='modal-content position-relative'>" +
            "                 <div class='position-absolute top-0 end-0 mt-2 me-2 z-index-1'>" +
            "                     <button class='btn-close btn btn-sm btn-circle d-flex flex-center transition-base' aria-label='Close' id='close_btn2' onclick ='closeRemoteModalNotRefresh()'></button>" +
            "                 </div>" +
            "                 <div class='modal-body p-0'>" +
            "                     <div class='rounded-top-lg py-3 ps-3 pe-6 bg-light'>" +
            "                         <span class='mb-1 fs-1 fw-semi-bold' id = 'remoteModelNotRefreshTitle'> Đoạn văn bản </span>" +
            "                     </div>" +
            "                     <div class='p-0 pb-0' id='remoteModelNotRefreshContent' >" +
            " <div class='m-2 post-summary' > " +
            "         <div class=''>" +
            renderDocName(doc) +
            "</div>" +
            "         <div class='text-600'>" +
            replaceText(doc) +
            "</div>" +
            "         <div>Ngày hiệu lực: " +
            dateToddMMyyyyHHmmss(doc.effectDate) +
            "         </div>" +
            (doc.expireDate != null
              ? "         <div>Ngày hết hạn: " +
                dateToddMMyyyyHHmmss(doc.expireDate) +
                "         </div>"
              : "") +
            "         <div class='mt-3'>" +
            "           <md-block>" +
            displayString(
              doc["chunkContent"] ??
                doc["docItemContentFull"] ??
                doc["docItemContent"]
            ) +
            "</md-block>" +
            "         </div>" +
            " <div>" +
            "                 </div>" +
            "             </div>" +
            "         </div>" +
            "     </div>";
          $("html").append(content);
          $("#close_btn2").on("click", function () {
            $("#remoteModelNotRefresh3").remove();
          });
        } else {
          makeToast("error", result.message);
        }
      },
    });
  }
}
var originFieldId = 0;
var listStatusField = [
  { id: 0, text_color: "black", color: "", name: "" },
  { id: 1, text_color: "black", color: "bg-success", name: "Hoạt động" },
  { id: 2, text_color: "black", color: "bg-danger", name: "Dừng hoạt động" },
  { id: 3, text_color: "black", color: "bg-warning", name: "Thử nghiệm" },
  {
    id: 4,
    text_color: "black",
    color: "bg-secondary",
    name: "Đang huấn luyện",
  },
  { id: 5, text_color: "black", color: "bg-info", name: "Đã huấn luyện" },
  {
    id: 6,
    text_color: "black",
    color: "bg-secondary",
    name: "Chờ Import DocItems",
  },
  {
    id: 7,
    text_color: "black",
    color: "bg-secondary",
    name: "Đã Import DocItems",
  },
  { id: 8, text_color: "black", color: "bg-danger", name: "Đã xóa" },
];
function GetFullnamePrefix(fullname) {
  var retVal = "";
  if (!fullname) return retVal;
  fullname = fullname.trim();
  while (fullname.includes("  ")) {
    fullname = fullname.replace("  ", " ");
  }
  if (fullname.includes(" ")) {
    var splits = fullname.split(" "); // Tách chuỗi thành mảng các phần từ dựa trên khoảng trắng
    retVal = splits[0][0] + splits[splits.length - 1][0]; // Lấy ký tự đầu của phần tử đầu tiên và cuối cùng
  } else {
    if (fullname.length > 2) {
      retVal = fullname.substring(0, 2);
    } else {
      retVal = fullname;
    }
  }
  return retVal;
}
var crrFieldDocId = 0;
var docChunks = [];
function getDocChunkHtmlRow(docChunk) {
  var css_class = "";
  if ($("#addedDocChunk_" + docChunk.qaDocChunkId).length) {
    css_class = "edit_success_bg";
  }
  return `
                <div id="docChunk_${docChunk.qaDocChunkId}" data-id="${
    docChunk.qaDocChunkId
  }" class="docChunkItemToAdd row g-0 p-2 border-bottom border-bottom-2 hover-actions-trigger position-relative ${css_class}"><div class="col-12 chunk-content">
                <div class="fw-semi-bold text-600">${displayString(
                  docChunk.docName
                )}</div>
                <div class="text-600">${displayString(docChunk.lead)
                  .replaceAll("\r\n", "<br/>")
                  .replaceAll("\\r\\n", "<br/>")}</div>
                <div class="mt-3">${displayString(docChunk.chunkContent)}</div>
                <div class="hover-actions end-0 top-0 px-3 py-1 translate-middle-y bg-success" style="top:15px !important;">
                <button onclick="onAddDocChunk(${
                  docChunk.qaDocChunkId
                })" class="btn p-0 text-white" type="button" data-bs-toggle="" data-bs-placement="top" title="Thêm"><span class="fas fa-plus" style="color:var(--falcon-white) !important;"></span></button>
                </div>
                </div>
                </div>`;
}
function renderAvatar(qs) {
  return `<div class="avatar avatar-xl" style="width:1.3rem !important; height: 1.3rem !important;" role="button" data-bs-toggle="tooltip" title="${
    (qs.customerAccount ?? "Anonymous") +
    "/" +
    (qs.customerFullname ?? "Không xác định")
  }"> <div class="avatar-name rounded-circle ${
    lvnAccount.includes(qs.customerAccount) == true ? "bg-warning" : ""
  }" style=" font-size: 10px !important;"><span>${GetFullnamePrefix(
    qs.customerFullName ?? qs.customerAccount ?? "Không xác định"
  )}</span></div></div>`;
}
var listAddMultipleDocChunk = [];
function loadDocChunkToAdd() {
  $("#doc-chunks-container").html(
    `<div class="row"><div class="col d-flex justify-content-center"><div class="spinner-grow spinner-grow-sm" role="status"><span class="visually-hidden">Đang tải dữ liệu...</span></div></div></div>`
  );
  $.ajax({
    url:
      "/QAFieldDoc/GetDocChunksByFieldDocIdAjax?fieldDocId=" +
      crrFieldDocId +
      "&keyword=" +
      (crrFieldDocId == -1 ? originFieldId : ""),
    type: "GET",
    success: function (result) {
      listAddMultipleDocChunk = [];
      if (result.isSuccessed) {
        docChunks = result.resultObj;
        if (docChunks == null || docChunks.length <= 0) {
          $("#doc-chunks-container").html(
            `<div class="row"><div class="col d-flex justify-content-center text-600">Văn bản chưa được tách.</div></div>`
          );
        } else {
          $("#doc-chunks-container").html("");
          for (var i = 0; i < docChunks.length; i++) {
            $("#doc-chunks-container").append(getDocChunkHtmlRow(docChunks[i]));
          }
          let firstSelected = null; // Lưu đoạn đầu tiên được click
          $(".docChunkItemToAdd").each(function (index) {
            $(this).on("click", function (event) {
              event.preventDefault(); // Ngăn chặn hành vi mặc định của trình duyệt
              if (event.target.tagName != "svg") {
                if (!$(this).hasClass("edit_success_bg")) {
                  if (event.shiftKey && firstSelected !== null) {
                    // Nếu giữ phím Shift và đã có đoạn đầu tiên
                    let start = Math.min(firstSelected, index);
                    let end = Math.max(firstSelected, index);

                    // Chọn tất cả các đoạn từ firstSelected đến đoạn hiện tại
                    for (let i = start; i <= end; i++) {
                      let $chunkElem = $(".docChunkItemToAdd").eq(i);
                      if (!$chunkElem.hasClass("bg-docchunk-seleteced")) {
                        $chunkElem.addClass("bg-docchunk-seleteced");
                        listAddMultipleDocChunk.push($chunkElem.data("id"));
                      }
                    }
                  } else {
                    // Nếu không giữ phím Shift, toggle chọn đoạn hiện tại
                    $(this).toggleClass("bg-docchunk-seleteced");
                    if ($(this).hasClass("bg-docchunk-seleteced")) {
                      listAddMultipleDocChunk.push($(this).data("id"));
                    } else {
                      listAddMultipleDocChunk = listAddMultipleDocChunk.filter(
                        (x) => x != $(this).data("id")
                      );
                    }
                    firstSelected = index; // Lưu đoạn hiện tại làm đoạn đầu tiên
                  }
                }
              }
              $(".btn-add-multiple-docchunks").remove();
              if (
                $(".btn-add-multiple-docchunks").length <= 0 &&
                listAddMultipleDocChunk != null &&
                listAddMultipleDocChunk.length > 0
              ) {
                $("#add-docchunks-container").append(
                  `<div class="end-0 px-3 py-1 translate-middle-y bg-primary btn-add-multiple-docchunks d-flex" onclick="onAddDocChunk(0,true)" style="position: absolute;top: 96.5% !important;">
                                        <span class="px-1 text-white d-flex align-items-center">Đã chọn ${listAddMultipleDocChunk.length} đoạn văn bản</span>
                                        <button class="btn p-0 px-3 py-1 text-white bg-success" type="button" data-bs-toggle="" data-bs-placement="top" title="Thêm nhiều đoạn văn bản">
                                            <svg class="svg-inline--fa fa-plus fa-w-14" style="color: var(--falcon-white) !important;" aria-hidden="true" focusable="false" data-prefix="fas" data-icon="plus" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" data-fa-i2svg=""><path fill="currentColor" d="M416 208H272V64c0-17.67-14.33-32-32-32h-32c-17.67 0-32 14.33-32 32v144H32c-17.67 0-32 14.33-32 32v32c0 17.67 14.33 32 32 32h144v144c0 17.67 14.33 32 32 32h32c17.67 0 32-14.33 32-32V304h144c17.67 0 32-14.33 32-32v-32c0-17.67-14.33-32-32-32z"></path></svg><!-- <span class="fas fa-plus" style="color:var(--falcon-white) !important;"></span> Font Awesome fontawesome.com -->
                                        </button>
                                </div>`
                );
              }
            });
          });
        }
      } else {
        $("#doc-chunks-container").html(
          `<div class="row"><div class="col d-flex justify-content-center text-600">${result.message}</div></div>`
        );
      }
    },
    error: function (err) {
      openAlertModal("Lỗi", JSON.stringify(err));
      $("#doc-chunks-container").html(
        `<div class="row"><div class="col d-flex justify-content-center text-600">Lỗi tải dữ liệu, bạn vui lòng thử lại!</div></div>`
      );
    },
  });
}
function reloadDocOfField() {
  var fieldId = $("#selectDocField").val();
  loadDocsByField(fieldId);
}

function loadDocsByField(fieldId) {
  $.ajax({
    url: "training/getdocchunkbyfieldid",
    data: { field_id: fieldId },
    type: "get",
    success: function (response) {
      $("#ddlFieldDocs").html("");
      if (response && response.length > 0) {
        response.forEach((x) => {
          var html =
            '<option value="' +
            x.value +
            '" class="text-truncate" >' +
            x.text +
            "</option>";
          $("#ddlFieldDocs").append(html);
        });
      }
    },
    error: function (err) {
      console.log(err);
    },
  });
}

function ddlFieldDocs_OnChange() {
  var fieldDocId = $("#ddlFieldDocs").val();
  if (fieldDocId == crrFieldDocId) {
    return;
  }
  crrFieldDocId = fieldDocId;
  loadDocChunkToAdd();
}
function createFeedbackHTML(feedbackObj) {
  var listFbTag = [
    { id: 1, name: "Chính xác" },
    { id: 2, name: "Dễ hiểu" },
    { id: 3, name: "Hoàn chỉnh" },
    { id: 4, name: "Chưa chính xác" },
    { id: 5, name: "Sơ sài" },
    { id: 6, name: "Khác" },
  ];
  var html =
    '<div class="row gx-0 border-bottom border-bottom-1 py-2">' +
    '<div class="col">' +
    '<div class="row gx-0 py-1 justify-content-start">' +
    '<div class="col-auto d-flex align-items-center pe-2">' +
    '<span class="far ' +
    (feedbackObj.feedbackEmotionId == 1
      ? "fa-thumbs-up text-primary"
      : "fa-thumbs-down text-danger") +
    ' fs-1"></span>' +
    "</div>" +
    '<div class="col-auto d-flex align-items-center">' +
    (feedbackObj.feedbackTagId != null
      ? '<div class="btn btn-falcon-default rounded-pill py-0 m-0 fs--1" type="button">' +
        listFbTag.filter((x) => x.id == feedbackObj.feedbackTagId)[0].name +
        "</div>"
      : "") +
    '<div class="col d-flex align-items-center text-700 justify-content-end">' +
    dateToddMMyyyyHHmmss(feedbackObj.crDateTime) +
    "</div>" +
    "</div>" +
    '<div class="row gx-0 mt-2">' +
    feedbackObj.feedbackContent +
    "</div>" +
    "</div>" +
    "</div>";
  return html;
}
function renderQuestionNeedToTrain(qs, index, HasCheckbox) {
  var avatar = renderAvatar(qs);
  var fieldColor = listStatusField.filter((x) => x.id == qs.field.statusId)[0];
  var fields = `<span class="badge py-1 me-1 mb-1 ${
    fieldColor["color"] ?? "bg-success"
  }"> ${qs.field.qaFieldShortName} </span>`;
  var userTraining = "";

  if (qs.trainingUserId != 0 && qs.trainingUserId != null) {
    var userTrainingInfo = users.filter((x) => x.id == qs.trainingUserId)[0];
    //userTraining = `<span class="badge py-1 me-1 mb-1 bg-warning" data-bs-toggle="tooltip" title="câu hỏi ${qs.isTraining == 1 ? "đang" : "đã"} được huấn luyện bởi ${userTrainingInfo.userName} /${userTrainingInfo.userName}">${userTrainingInfo.userName}</span > `;
    if (
      userTrainingInfo != null &&
      userTrainingInfo.userName != null &&
      userTrainingInfo.userName.length > 0
    ) {
      userTraining =
        qs.isTraining == 1
          ? `(${userTrainingInfo.userName}/${userTrainingInfo.fullName} đang hl)`
          : `(${qs.questionTypeId == 6 ? "nhập kho" : "tách"} bởi ${
              userTrainingInfo.userName
            }/${userTrainingInfo.fullName})`;
    }
  }
  return `<tr>
                <td class="align-center px-1 text-center">${
                  index + 1 + " "
                }</td>
                ${
                  HasCheckbox == true
                    ? qs.isTraining == null
                      ? `<td class="px-0"><input type="checkbox" class="selection_question_box" id="select_qs_${qs.qaQuestionId}" name="select_qs_${qs.qaQuestionId}" value="${qs.qaQuestionId}"></td>`
                      : `<td class="px-0"></td>`
                    : ""
                }
                <td class="p-2">${avatar}</td>
                <td class="ps-0 pe-2 py-2 ${
                  qs.isTraining == 1 ? "" : "questionNeedToTrainClick"
                }${userTraining.length > 0 ? " text-700" : ""}" data-id='${
    qs.qaQuestionId
  }' data-trained="${
    qs.isTraining != 1 && qs.trainingUserId != null ? "1" : "0"
  }">
                    ${fields}${qs.questionContent}${userTraining}
                </td>
            </tr>`;
}

var qaAnswerId = null;
async function renderBody(res) {
  var qs = res.training;
  var qsnt = res.questionNeedToTrain;
  var nqsnt = res.numberOfQuestionsNeedToTrain;
  var qsTrained = res.questionTrained;
  var nbofqsTrained = res.numberOfQuestionTrained;
  if (qs != null) {
    qaquestionid = qs.qaQuestionId;
    originFieldId = qs.qaFieldId;
    $(".question-content").html("");
    $(".edit-training-question").html(
      '<span id="relevantQuestionEditButton_1" class="fas fa-edit text-500 ms-1 mx-1 cursor-pointer" style="font-size:0.8rem !important;" onclick="openEdit(' +
        qs.qaQuestionId +
        ')" title="Cập nhật nội dung câu hỏi"></span>'
    );
    getRelevantQuestion(qs.qaFieldId, qs.questionContent, false);
    getDocChunk(qs.qaQuestionId);
    var fieldColor = listStatusField.filter(
      (x) => x.id == qs.field.statusId
    )[0];
    /// lĩnh vực
    $("#selectDocField").val(qs.qaFieldId);
    $(".select2-selection__rendered").html(qs.field.qaFieldName);
    loadDocsByField(qs.qaFieldId);
    $(".question-field").html(
      `<span class="badge py-1 me-1 mb-1 ${
        fieldColor["color"] ?? "bg-success"
      }"> ${qs.field.qaFieldShortName} </span>`
    );
    var selectedValues = [];

    $('input[name="checkbox_field"]:checked').each(function () {
      selectedValues.push($(this).val());
    });

    $(".question-content").html(
      '<span class="QuestionContent_' +
        qs.qaQuestionId +
        '" data-bs-toggle="tooltip" title="' +
        qs.questionContent +
        '">' +
        qs.questionContent +
        "</span>"
    );
    $(".question-time").html(dateToddMMyyyyHHmmss(qs.crDateTime));
    searchContent = qs.questionContent;
    // $('#searchQuestion').val(qs.questionContent)
    var listStatusAnswer = [
      { id: 0, color: "", name: "" },
      { id: 1, color: "success", name: "Đã tinh chỉnh" },
      { id: 2, color: "secondary", name: "Mới tạo" },
      { id: 3, color: "danger", name: "Không phù hợp" },
    ];
    if (qs.answer && qs.answer.qaAnswerId > 0) {
      if (qs.answer.statusId == null) {
        qs.answer.statusId = 0;
      }
      qaAnswerId = qs.answer.qaAnswerId;
      var statusAnswer = listStatusAnswer.filter(
        (x) => x.id == qs.answer.statusId
      )[0];
      var answerContent =
        qs.answer.answerContent != null
          ? qs.answer.answerContent
          : "Không có nội dung câu trả lời";
      var contentKatex = renderKatex(answerContent);
      var contentRender = strDoc(contentKatex);
      $(".answer-status-badge").html(
        `<span class="badge py-0 my-0 badge-soft-${statusAnswer.color}">${statusAnswer.name}</span>`
      );
      $(".question-answer").append(`<md-block>${contentRender}</md-block>`);
    } else {
      $(".question-answer").html(
        "<md-block>Không có nội dung câu trả lời</md-block>"
      );
    }
    if (qs.relatedQuestions) {
      $(".related-question").html("");
      qs.relatedQuestions.forEach((x) => {
        var content =
          "" +
          '<tr class="Question_' +
          x.qaQuestionId +
          '">' +
          ' <td class="p-1 align-items-center">' +
          '     <div><span class="training-text QuestionContent_' +
          x.qaQuestionId +
          '">' +
          x.questionContent +
          "</span>" +
          '     <span class="text-600">(' +
          x.rankingScore +
          ") </span>" +
          '     <span style="width:max-content"><span class="cursor-pointer" onclick="openGetDocChunk(' +
          x.qaQuestionId +
          ')" title="' +
          x.chunkCounter +
          ' đoạn văn bản hướng dẫn trả lời">' +
          '         <span style="position: relative; display:inline-block"><div>' +
          '             <span class="fas fa-stop text-500 ms-1" ></span>' +
          '             <span class="text-white fw-semi-bold" style="font-size:10px !important; position: absolute; top:-1px; right:0px">' +
          (x.chunkCounter > 9 ? x.chunkCounter : "0" + x.chunkCounter) +
          "             </span></div>" +
          "         </span>" +
          "     </span>" +
          '     <span id="relevantQuestionEditButton_1" class="fas fa-edit text-500 ms-1 cursor-pointer" style="font-size:0.8rem !important;" onclick="openEdit(' +
          x.qaQuestionId +
          ')" title="Cập nhật nội dung câu hỏi"></span>' +
          " </span></div</td>" +
          ' <td class="p-1 cursor-pointer btn-copy" onclick="CopyDocChunk(' +
          x.qaQuestionId +
          ')" data-id=' +
          x.qaQuestionId +
          ' title = "Sao chép các đoạn văn bản" > ' +
          '     <span class="far fa-copy text-700 fs-1"></span>' +
          " </td>" +
          "</tr>";
        $(".related-question").append(content);
      });
    }
    // avatar
    var avatarContent = renderAvatar(qs);
    $(".user-avatar").empty();
    $(".user-avatar").append(avatarContent);

    if (qs.feedbacks && qs.feedbacks.length > 0) {
      qs.feedbacks.forEach((x) => {
        var feedbackHTML = createFeedbackHTML(x);
        $(".feedback-content").append(feedbackHTML);
      });
      $(".feedback-count").html(qs.feedbacks.length);
    } else {
      var html =
        '<div class="row gx-0 border-bottom border-bottom-1 py-2"><div class="col"><div class="row gx-0 py-1 justify-content-start"><div class="col-auto d-flex align-items-center pe-2"></div><div class="col-auto d-flex align-items-center"></div><div class="col d-flex align-items-center text-700 justify-content-end"></div></div><div class="row gx-0 mt-2">Không có đánh giá</div></div></div>';
      $(".feedback-content").append(html);
    }
    //setTimeout(() => {
    //    getRelevantQuestionInterval();
    //}, 500);
  } else {
    if (res.extension == 999) {
      makeToast("success", res.message);
      $(".btn-modal-field").click();
    } else {
      openAlertModal("Thông báo!", res.message);
    }
  }
  if (qsnt != null && qsnt.length > 0) {
    $("#questionNeedToTrain").html("");
    qsnt.forEach((x, i) => {
      var htmlContent = renderQuestionNeedToTrain(x, i, true);
      $("#questionNeedToTrain").append(htmlContent);
    });
    $(".question-need-train").html(nqsnt);
    $(".questionNeedToTrainClick").on("click", function () {
      var id = $(this).data("id");
      getDataRequest.questionId = id;
      getQuestionTraining();
    });
  }
  if (qsTrained != null && qsTrained.length > 0) {
    $("#questionTrained").html("");
    qsTrained.forEach((x, i) => {
      var htmlContent = renderQuestionNeedToTrain(x, i);
      $("#questionTrained").append(htmlContent);
    });
    $(".question-trained").html(nbofqsTrained);
    $(".questionNeedToTrainClick").on("click", function () {
      var id = $(this).data("id");
      var trained = $(this).data("trained");
      if (trained == 1 || trained == "1" || trained == "1") {
        resetConfirmButton();
        $("#confirmModelButton").on("click", {}, function () {
          resetConfirmButton();
          closeConfirmModal();
          getDataRequest.questionId = id;
          getQuestionTraining();
        });
        openConfirmModal("Đồng ý", "Bạn muốn huấn luyện lại câu hỏi này?");
      } else {
        getDataRequest.questionId = id;
        getQuestionTraining();
      }
    });
  }
  if (res.waitingTrain != null && res.waitingTrain.length > 0) {
    $("#questionWaitingToTrain").html("");
    res.waitingTrain.forEach((x, i) => {
      var htmlContent = renderQuestionNeedToTrain(x, i);
      $("#questionWaitingToTrain").append(htmlContent);
    });
    $(".question-waiting-train").html(res.waitingCount);
    $(".questionNeedToTrainClick").on("click", function () {
      var id = $(this).data("id");
      getDataRequest.questionId = id;
      getQuestionTraining();
    });
  }
  isLoadingNewQuestion = false;
}
var qaquestionid = null;
var isLoadingNewQuestion = true;
function getQuestionTraining() {
  isLoadingNewQuestion = true;
  isCanLoadNeedToTrainRequest = true;
  isLoadingNeedToTrainRequest = false;
  isCanLoadWaitingToTrainRequest = true;
  isLoadingWaitingToTrainRequest = false;
  getNeedToTrainRequest = JSON.parse(JSON.stringify(getDataRequest));
  $(".edit-training-question").html("");
  $(".user-avatar").html("");
  $(".question-field").html("");
  $(".answer-status-badge").html("");
  $(".nav-link").removeClass("active");
  $(".tab-pane").removeClass("show").removeClass("active");
  $("#question-tab").addClass("active");
  $("#tab-question").addClass("active").addClass("show");
  $("#doc-chunks-container").html("");
  $(".question-content").html(
    `<div class="row"><div class="col d-flex justify-content-center"><div class="spinner-grow spinner-grow-sm" role="status"><span class="visually-hidden">Đang tải dữ liệu...</span></div></div></div>`
  );
  $(".question-time").html("");
  $(".question-answer").html("");
  $(".docchunk").html(
    `<div class="row"><div class="col d-flex justify-content-center"><div class="spinner-grow spinner-grow-sm" role="status"><span class="visually-hidden">Đang tải dữ liệu...</span></div></div></div>`
  );
  $(".feedback-content").html(
    `<div class="row"><div class="col d-flex justify-content-center"><div class="spinner-grow spinner-grow-sm" role="status"><span class="visually-hidden">Đang tải dữ liệu...</span></div></div></div>`
  );
  $(".related-question").html(
    `<div class="row"><div class="col d-flex justify-content-center"><div class="spinner-grow spinner-grow-sm" role="status"><span class="visually-hidden">Đang tải dữ liệu...</span></div></div></div>`
  );
  $(".related-pri-sec-question").html(
    `<div class="row"><div class="col d-flex justify-content-center"><div class="spinner-grow spinner-grow-sm" role="status"><span class="visually-hidden">Đang tải dữ liệu...</span></div></div></div>`
  );
  $(".btn-allow-question").hide();
  $("#questionTrained").html(
    '<span class="px-2">Bạn chưa huấn luyện câu hỏi nào!</span>'
  );
  $(".question-trained").html("0");
  $("#questionWaitingToTrain").html(
    '<span class="px-2">Không có câu hỏi huấn luyện sau!</span>'
  );
  $(".question-waiting-train").html("0");
  $("#questionNeedToTrain").html(
    '<span class="px-2">Không có câu hỏi cần huấn luyện!</span>'
  );
  $(".question-need-train").html("0");
  $(".question-answer-internet").html("Câu trả lời từ ChatGPT");
  $(".question-answer-now").html("Câu trả lời từ AILuat");
  $(".feedback-content").html("");
  $.ajax({
    url: "training/GetTrainingData",
    data: getDataRequest,
    type: "POST",
    success: function (result) {
      renderBody(result);
      $(".btn-split-qs").prop("disabled", false);
    },
    error: function (err) {
      console.log(err);
    },
  });
}
var now = new Date();
var year = now.getFullYear(); // Lấy năm
var month = now.getMonth() + 1; // Lấy tháng (từ 0 đến 11, nên cộng thêm 1)
var day = now.getDate(); // Lấy ngày trong tháng
function generateHTML(quest) {
  function processDocItemRelates(docItemRelates, indentLevel = 2) {
    let x = "";

    if (docItemRelates != null && docItemRelates.length > 0) {
      x += `<div class="col-12 chunk-content ps-${indentLevel}">`;
      x += '<div class="col border-bottom pt-3 pb-2 fw-bold">Sửa đổi</div>';
      docItemRelates.forEach((rItem) => {
        x += `<div class="col-12 chunk-content ps-${indentLevel} mt-2">`;
        x += `<div class="fw-semi-bold text-600">Tác động: ${
          rItem.relateTypeName ?? ""
        }</div>`;
        x += `<div class="fw-semi-bold text-600">${rItem.docName ?? ""}</div>`;
        x +=
          '<div class="fw-semi-bold text-600">Ban hành: ' +
          (rItem.effectDate
            ? new Date(quest.effectDate).toLocaleDateString("vi-VN")
            : "") +
          "</div>";
        x +=
          '<div class="fw-semi-bold text-600">Hết hiệu lực: ' +
          (rItem.expireDate
            ? new Date(quest.expireDate).toLocaleDateString("vi-VN")
            : "") +
          "</div>";
        x += `<div class="text-600">${
          rItem.docItemParentLead
            ? rItem.docItemParentLead
                .split(/\\r\\n|\r\n|\r|\n|\\n/)
                .join("<br/>")
            : ""
        }</div>`;
        x += `<div class="mt-3">${rItem.docItemContent ?? ""}</div>`;
        if (rItem.docItemRelates != null && rItem.docItemRelates.length > 0) {
          x += processDocItemRelates(rItem.docItemRelates, indentLevel + 2);
        }
        x += "</div><hr/>";
      });
      x += "</div>";
    }
    return x;
  }
  var crdate = new Date(quest.docChunkQuestionCrDateTime);
  var crUser =
    users.filter((x) => x.id == quest.addedInTrainBy).length > 0
      ? users.filter((x) => x.id == quest.addedInTrainBy)[0]
      : null;
  var isTodayAdded =
    crdate.getFullYear() == now.getFullYear() &&
    crdate.getMonth() + 1 == now.getMonth() + 1 &&
    crdate.getDate() == now.getDate() &&
    quest.qaDocChunkId > 0;
  var html = "";
  html +=
    '<div id="addedDocChunk_' +
    quest.qaDocChunkId +
    '" data-id="' +
    quest.qaDocChunkId +
    '" class="row p-2 g-0 addedDocChunk border-bottom border-bottom-2 hover-actions-trigger position-relative ' +
    (quest.qaDocChunkId <= 0 ? "bg-200" : "") +
    (isTodayAdded ? "edit_success_bg" : "") +
    '">';
  html += '<div class="col-12 chunk-content">';
  html +=
    '<div class="fw-semi-bold text-600">' + (quest.docName ?? "") + "</div>";
  html +=
    '<div class="fw-semi-bold text-600">Ban hành: ' +
    (quest.effectDate
      ? new Date(quest.effectDate).toLocaleDateString("vi-VN")
      : "") +
    "</div>";
  html +=
    '<div class="fw-semi-bold text-600">Hết hiệu lực: ' +
    (quest.expireDate
      ? new Date(quest.expireDate).toLocaleDateString("vi-VN")
      : "") +
    "</div>";
  html +=
    '<div class="text-600">' +
    (quest.lead ? quest.lead.split(/\\r\\n|\r\n|\r|\n|\\n/).join("<br/>") : "");
  html += "</div>";
  html += '<div class="mt-3">' + (quest.chunkContent ?? "") + "</div>";
  if (quest.qaDocChunkId > 0) {
    html +=
      '<div class="hover-actions end-0 top-0 px-3 py-1 translate-middle-y  bg-danger" style="top:15px !important;">';

    html +=
      '<button onclick="onRemoveDocChunk(' +
      quest.qaDocChunkId +
      ')" class="btn p-0 px-1 mx-1 text-white" type="button" data-bs-toggle="" data-bs-placement="top" title="Xóa/(' +
      (crUser != null
        ? "Được thêm bởi " + crUser.fullName + "/" + crUser.userName
        : "Không xác định người tạo do dữ liệu quá cũ") +
      ')"><span class="fas fa-times" style="color:var(--falcon-white) !important;"></span></button>';
    html += "</div>";
  }
  if (quest.qaDocChunkId > 0 && quest.docChunkSourceId == 4) {
    html +=
      '<div class="hover-actions end-4 top-0 px-3 py-1 translate-middle-y  bg-primary" style="top:15px !important;">';

    html +=
      '<button onclick="editContentDocChunk(' +
      quest.qaDocChunkId +
      ')" class="btn p-0 px-1 mx-1 text-white" type="button" data-bs-toggle="" data-bs-placement="top" title="Sửa nội dung/(' +
      (crUser != null
        ? "Được thêm bởi " + crUser.fullName + "/" + crUser.userName
        : "Không xác định người tạo do dữ liệu quá cũ") +
      ')"><span class="fas fa-edit" style="color:var(--falcon-white) !important;"></span></button>';
    html += "</div>";
  }
  if (quest.docItemRelates != null && quest.docItemRelates.length > 0) {
    html += processDocItemRelates(quest.docItemRelates);
  }
  html += "</div></div>";
  return html;
}
function getDocChunk(questionId) {
  $(".docchunk").html(
    `<div class="row"><div class="col d-flex justify-content-center"><div class="spinner-grow spinner-grow-sm" role="status"><span class="visually-hidden">Đang tải dữ liệu...</span></div></div></div>`
  );
  $.ajax({
    url: "/training/getdocchunk?id=" + questionId,
    method: "get",
    success: function (response) {
      // Xử lý phản hồi từ máy chủ
      if (response) {
        var count = 0;
        $(".docchunk").html("");
        $(".doc-chunk-count").html(0);
        if (response && response.length > 0) {
          response.forEach((x) => {
            var html = generateHTML(x);
            if (x.qaDocChunkId != 0 && x.qaDocChunkId != null) {
              count++;
            }
            $(".docchunk").append(html);
          });
          $(".doc-chunk-count").html(count);
        } else {
          $(".docchunk").append("<div>Chưa có đoạn văn bản hướng dẫn</div>");
        }
      }
    },
    error: function (xhr, status, error) {
      // Xử lý lỗi nếu có
      console.error(error);
    },
  });
}

function openGetDocChunk(id) {
  openRemoteModalNotRefresh(
    `qadocchunk/getdocchunk?id=${id}`,
    "Đoạn văn bản hướng dẫn trả lời",
    "99%",
    "92%"
  );
}

function openEdit(id) {
  openRemoteModalNotRefresh(
    `qaquestion/editcontent/${id}`,
    "Cập nhật nội dung câu hỏi",
    "500px",
    "200px"
  );
}
function openDelete(id) {
  openRemoteModalNotRefresh(
    `qaquestion/delete/${id}`,
    "Xóa câu hỏi",
    "500px",
    "200px"
  );
}
function RemoveQuestion(id) {
  $(".Question_" + id).remove();
  onCancel_Click(false);
}
$("#searchQuestion").on("keydown", function (event) {
  if (event.key === "Enter") {
    getRelevantQuestion(
      originFieldId,
      $("#searchQuestion").val() == null || $("#searchQuestion").val() == ""
        ? searchContent
        : $("#searchQuestion").val()
    );
  }
});
$("#searchQuestion2").on("keydown", function (event) {
  if (event.key === "Enter") {
    isLoadingNeedToTrainRequest = false;
    getNeedToTrainRequest.keyWord = $("#searchQuestion2").val();
    getNeedToTrainRequest.pageIndex = 1;
    getNeedToTrainRequest.isLoadNeedToTrain = true;
    $("#questionNeedToTrain").html("");
    isCanLoadNeedToTrainRequest = true;
    loadMoreTrainingData(
      "NeedToTrain",
      getNeedToTrainRequest,
      "questionNeedToTrainLoading",
      "#questionNeedToTrain",
      ".question-need-train",
      renderQuestionNeedToTrain
    );
  }
});
$("#searchQuestion3").on("keydown", function (event) {
  if (event.key === "Enter") {
    isLoadingWaitingToTrainRequest = false;
    getWaitingToTrainRequest.keyWord = $("#searchQuestion3").val();
    getWaitingToTrainRequest.pageIndex = 1;
    getWaitingToTrainRequest.isLoadWaitingToTrain = true;
    $("#questionWaitingToTrain").html("");
    isCanLoadWaitingToTrainRequest = true;
    loadMoreTrainingData(
      "WaitingToTrain",
      getWaitingToTrainRequest,
      "questionWaitingToTrainLoading",
      "#questionWaitingToTrain",
      ".question-waiting-train",
      renderQuestionNeedToTrain
    );
  }
});
var isRelevantLoading = false;
function getRelevantQuestionInterval() {
  setInterval(() => {
    if (!isRelevantLoading) {
      getRelevantQuestion(
        originFieldId,
        $("#searchQuestion").val() == null || $("#searchQuestion").val() == ""
          ? searchContent
          : $("#searchQuestion").val(),
        true
      );
    }
  }, 10000);
}
function getRelevantQuestion(
  fieldId,
  trainedQuestion,
  isLoadingInterval = false
) {
  isRelevantLoading = true;
  var trainingClass = $(".related-pri-sec-question");
  if (!isLoadingInterval) {
    $(".related-pri-sec-question").html(
      `<div class="row"><div class="col d-flex justify-content-center"><div class="spinner-grow spinner-grow-sm" role="status"><span class="visually-hidden">Đang tải dữ liệu...</span></div></div></div>`
    );
    $(".new-div").remove();
    $("#use-document-button").show();
  } else {
    $(".primary-loading").addClass("bg-success");
  }

  $.ajax({
    url:
      "/Training/GetRelevantPrimarySecondaryQuestionsPaging?fieldId=" +
      fieldId +
      "&question_content=" +
      trainedQuestion,
    method: "get",
    success: function (response) {
      // Xử lý phản hồi từ máy chủ
      if (!isLoadingInterval) {
        trainingClass.html("");
      }
      var trainingQuestion = response.resultObj;
      function showTrainingQuestion(item, index) {
        var childHTML =
          "" +
          '<tr class="Question_' +
          item.qaquestion_id +
          '">' +
          ' <td class="p-1 align-items-center">' +
          '     <div><span class="training-text QuestionContent_' +
          item.qaquestion_id +
          '">' +
          item.question_content +
          "</span>" +
          '     <span style="width:max-content"><span class="cursor-pointer" onclick="openGetDocChunk(' +
          item.qaquestion_id +
          ')" title="Đoạn văn bản hướng dẫn trả lời">' +
          '         <span style="position: relative; display:inline-block"><div>' +
          '             <span class="fas fa-stop text-500 ms-1"></span>' +
          '             <span class="text-white fw-semi-bold" style="font-size:10px !important; position: absolute; top:-1px; right:0px">' +
          (item.chunk_counter > 9
            ? item.chunk_counter
            : "0" + item.chunk_counter) +
          "             </span></div>" +
          "         </span>" +
          "     </span>" +
          '     <span id="relevantQuestionEditButton_1" class="fas fa-edit text-500 ms-1 cursor-pointer" style="font-size:0.8rem !important;" onclick="openEdit(' +
          item.qaquestion_id +
          ')" title="Cập nhật nội dung câu hỏi"></span>' +
          '     <span id="relevantQuestionEditButton_1" class="fas fa-trash text-500 ms-1 cursor-pointer" style="font-size:0.8rem !important;" onclick="openDelete(' +
          item.qaquestion_id +
          ')" title="Xóa câu hỏi"></span>' +
          (item.question_type_id == 1
            ? ' <span class="fas fa-key fs--2 text-500" title="Câu hỏi chính"></span>'
            : ' <span class="fas fa-code-branch fs--2 text-500" title="Câu hỏi phụ"></span>') +
          " </span></div</td>" +
          ' <td class="p-1 cursor-pointer btn-copy" onclick="CopyDocChunk(' +
          item.qaquestion_id +
          ')" data-id=' +
          item.qaquestion_id +
          ' title="Sao chép các đoạn văn bản">' +
          '  <span class="far fa-copy text-700 fs-1"></span>' +
          " </td>" +
          ' <td class="p-1" title="Chọn làm câu hỏi chính">' +
          '     <div class="form-check form-switch m-0 mh-0">' +
          '         <input class="form-check-input" id="flexSwitchCheckChecked" type="checkbox" data-id="' +
          item.qaquestion_id +
          '"/>' +
          "     </div>" +
          " </td>" +
          "</tr>";
        if (!isLoadingInterval) {
          trainingClass.append(childHTML);
        } else {
          if (isLoadingNewQuestion != true) {
            var check = $(".Question_" + item.qaquestion_id);
            var ishaveQuestion = check && check.length > 0 ? true : false;
            if (ishaveQuestion == false) {
              $(".related-pri-sec-question")
                .children()
                .eq(index)
                .before(childHTML);
              $(".Question_" + item.qaquestion_id).addClass("bg-soft-success");
            }
          }
        }
      }
      var newDiv = document.createElement("div");
      newDiv.className = "new-div";
      newDiv.style.position = "absolute";
      newDiv.style.top = "0";
      newDiv.style.left = "0";
      newDiv.style.width = "100%";
      newDiv.style.height = "100%";
      newDiv.style.backgroundColor = "rgba(0, 0, 0, 0.25)";
      newDiv.style.display = "flex";
      newDiv.style.justifyContent = "center";
      newDiv.style.alignItems = "center";
      newDiv.style.color = "rgb(255 255 255)";
      newDiv.style.zIndex = "9";
      newDiv.style.fontSize = "48px";

      // Thêm nội dung vào div mới
      var content = document.createElement("div");
      content.innerText = "Không sử dụng đoạn văn bản hướng dẫn";
      newDiv.appendChild(content);

      trainingQuestion.forEach(showTrainingQuestion);
      $(".form-check-input").on("click", function () {
        if ($(this).is(":checked")) {
          $(".form-check-input").not(this).prop("checked", false);
          $("#use-document").append(newDiv);
          $("#use-document-button").hide();
          $("#use-primary-question-button").show();
        } else {
          $("#use-primary-question-button").hide();
          newDiv.remove();
          $("#use-document-button").show();
        }
      });
      isRelevantLoading = false;
      $(".primary-loading").removeClass("bg-success");
    },
    error: function (xhr, status, error) {
      // Xử lý lỗi nếu có
      console.error(error);
    },
  });
}
function removeAllDocumentChunks() {
  resetConfirmButton();
  $("#confirmModelButton").on("click", {}, function () {
    resetConfirmButton();
    closeConfirmModal();
    $.ajax({
      url: "/Training/RemoveAllDocChunks/",
      method: "delete",
      data: { questionid: qaquestionid },
      success: function () {
        // Xử lý phản hồi từ máy chủ
        $(".row.table-responsive.scrollbar.docchunk").empty();
        $(
          ".row.g-0.p-2.border-bottom.border-bottom-2.hover-actions-trigger.position-relative.edit_success_bg"
        ).removeClass("edit_success_bg");
        makeToast("success", "Đã xóa đoạn văn bản hướng dẫn");
        getDocChunk(qaquestionid);
      },
      error: function (xhr, status, error) {
        // Xử lý lỗi nếu có
        console.error(error);
      },
    });
  });
  openConfirmModal(
    "Đồng ý",
    "Bạn muốn xóa toàn bộ đoạn văn bản hướng dẫn của câu hỏi này?"
  );
}

// Sửa sang post hoặc delete
function onRemoveDocChunk(docchunkid) {
  resetConfirmButton();
  $("#confirmModelButton").on("click", {}, function () {
    resetConfirmButton();
    closeConfirmModal();
    $.ajax({
      url:
        "/Training/RemoveDocChunk?questionid=" +
        qaquestionid +
        "&docchunkid=" +
        docchunkid,
      method: "get",
      success: function (response) {
        // Xử lý phản hồi từ máy chủ
        if (response.isSuccessed == true) {
          $(".row.table-responsive.scrollbar.docchunk")
            .find("#addedDocChunk_" + docchunkid)
            .remove();
          $("#docChunk_" + docchunkid).removeClass("edit_success_bg");
          makeToast("success", "Đã xóa đoạn văn bản hướng dẫn");
          getDocChunk(qaquestionid);
        } else {
          openAlertModal("Lỗi", response.message);
        }
      },
      error: function (xhr, status, error) {
        // Xử lý lỗi nếu có
        console.error(error);
      },
    });
  });
  openConfirmModal("Đồng ý", "Bạn muốn xóa đoạn văn bản này?");
}

function CopyDocChunk(questionId) {
  var copydocchunk = {
    originQAQuestionId: qaquestionid,
    QAQuestionId: questionId,
    CrUserId: 0,
  };
  var isSelectQuestion = false;
  $(".form-check-input").each(function () {
    if ($(this).is(":checked")) isSelectQuestion = true;
  });
  if (isSelectQuestion == true) {
    makeToast(
      "warning",
      "Bạn đang chọn câu hỏi chính!<br/> Vui lòng xóa câu hỏi chính trước!"
    );
    return;
  }
  $.ajax({
    url: "training/copydocchunk",
    data: copydocchunk,
    type: "POST",
    success: function (result) {
      if (result && !result["isSuccessed"]) {
        openAlertModal("Thông báo", result.message);
      } else if (result && result["isSuccessed"]) {
        makeToast("success", result.message);
        getDocChunk(qaquestionid);
      } else
        openAlertModal(
          "Thông báo",
          "Bạn không có quyền truy cập chức năng này!"
        );
    },
    error: function (err) {
      console.log(err);
    },
  });
}
$(".btn-allow-question")
  .off()
  .on("click", function () {
    var data = $(this).data("id");
    resetConfirmButton();
    $("#confirmModelButton").on("click", {}, function () {
      resetConfirmButton();
      closeConfirmModal();
      var selectedField = $("#QAFieldId").val();
      var multipleFieldId = selectedField;

      var selectedQuestionId = 0;
      $(".form-check-input").each(function () {
        if ($(this).is(":checked")) {
          selectedQuestionId = $(this).data("id");
        }
      });

      if (data == 1 && selectedQuestionId == 0) {
        makeToast("error", "Bạn chưa chọn câu hỏi chính");
        return;
      }
      var request = {
        originQAQuestionId: qaquestionid,
        QAQuestionId: data == 1 ? selectedQuestionId : "",
        AllowStatusId: 1,
        CrUserId: "",
        SyncFlag: 1,
        multipleFieldId: multipleFieldId,
        reUseAnswer: $('input[name="reUseAnswer"]').prop("checked"),
      };
      $.ajax({
        url: "training/allowbotusingquestion",
        method: "post",
        data: request,
        success: function (response) {
          // Xử lý phản hồi từ máy chủ
          if (response.isSuccessed == true) {
            makeToast("success", "Đã cho phép");
            getDataRequest.questionId = null;
            getQuestionTraining();
          } else {
            openAlertModal("Lỗi", response.message);
          }
        },
        error: function (xhr, status, error) {
          // Xử lý lỗi nếu có
          console.error(error);
        },
      });
    });
    openConfirmModal(
      "Đồng ý",
      "Bạn muốn đồng ý cho phép Chatbot sử dụng câu hỏi này?"
    );
  });

function onAddDocChunk(docChunkId, isMultiple = false) {
  var docChunk = null;
  function sendRequest(docChunkId, multiple = false) {
    var request = {
      qaDocChunkId: docChunkId,
      QAQuestionId: qaquestionid,
      IsMultipleDocChunk: multiple,
      ListDocChunkId:
        multiple == true ? listAddMultipleDocChunk.join(",") : null,
    };

    $.ajax({
      url: "/QAFieldDoc/AddDocChunkQuestion",
      data: request,
      type: "POST",
      success: function (result) {
        if (result.isSuccessed) {
          makeToast("success", "Thêm đoạn văn bản thành công");
          if (multiple) {
            listAddMultipleDocChunk.forEach((x) => {
              $("#docChunk_" + x).addClass("edit_success_bg");
            });
          }
          $(".btn-add-multiple-docchunks").remove();
          $(".docChunkItemToAdd").each(function (index) {
            $(this).removeClass("bg-docchunk-seleteced");
          });
          listAddMultipleDocChunk = [];
          $("#docChunk_" + docChunkId).addClass("edit_success_bg");
          getDocChunk(qaquestionid);
        } else {
          openAlertModal("Lỗi", result.message);
        }
      },
      error: function (err) {
        openAlertModal("Lỗi", JSON.stringify(err));
      },
    });
  }

  if (isMultiple != true) {
    for (var i = 0; i < docChunks.length; i++) {
      if (docChunks[i].qaDocChunkId == docChunkId) {
        docChunk = docChunks[i];
        break;
      }
    }
    if (docChunk == null) {
      openAlertModal("Lỗi", "Không tìm thấy đoạn văn");
      return;
    }

    if ($("#addedDocChunk_" + docChunkId).length) {
      openAlertModal("Lỗi", "Đoạn văn bản đã được thêm cho câu hỏi!");
      return;
    }
    sendRequest(docChunkId, false);
  } else {
    if (
      listAddMultipleDocChunk != null &&
      listAddMultipleDocChunk.length >= 0
    ) {
      sendRequest(0, true);
    }
    return;
  }
}
function ignoreTrainning(typeId, isMultiple) {
  resetConfirmButton();
  $("#confirmModelButton").on("click", {}, function () {
    resetConfirmButton();
    closeConfirmModal();
    var listData = [];
    if (isMultiple == true) {
      $(".selection_question_box").each(function () {
        if ($(this).is(":checked")) {
          listData.push($(this).val());
        }
      });
      if (listData.length == 0) {
        var message = "Vui lòng chọn câu hỏi trước";
        makeToast("error", message);
        openAlertModal("Thông báo", message);
        return;
      }
    }
    var postData = {
      QAQuestionId: qaquestionid,
      QuestionTypeId: typeId,
      IsMultiple: isMultiple,
      ListQuestionId: listData.join(","),
    };
    $.ajax({
      url: "/Training/IgnoreTraining",
      type: "POST",
      data: postData,
      success: function (result) {
        if (result.isSuccessed) {
          getDataRequest.questionId = null;
          getQuestionTraining();
          makeToast(
            "success",
            `${
              typeId == 5
                ? "Đã bỏ qua câu hỏi!"
                : "Đã cho câu hỏi vào kho chờ huấn luyện sau!"
            } `
          );
        } else {
          openAlertModal("Lỗi", result.message);
        }
      },
      error: function (err) {
        openAlertModal("Lỗi", JSON.stringify(err));
      },
    });
  });
  openConfirmModal(
    "Đồng ý",
    `Bạn thực sự muốn ${
      typeId == 5
        ? "bỏ qua(không huấn luyện) câu hỏi này"
        : " cho câu hỏi này vào kho chờ huấn luyện"
    } ?`
  );
}
function stopTraining() {
  resetConfirmButton();
  $("#confirmModelButton").on("click", {}, function () {
    resetConfirmButton();
    closeConfirmModal();
    getDataRequest.stopTraining = 1;
    $.ajax({
      url: "training/GetTrainingData",
      data: getDataRequest,
      type: "POST",
      success: function (result) {
        if (result.isSuccessed == true) {
          getDataRequest.stopTraining = null;
          makeToast("success", "Đã ngừng huấn luyện câu hỏi!");
          setTimeout(() => {
            window.location.href = "/home/index";
          }, 500);
        } else {
          openAlertModal("Lỗi", result.message);
        }
      },
      error: function (err) {
        console.log(err);
      },
    });
  });
  openConfirmModal(
    "Đồng ý",
    "Bạn thực sự muốn ngừng huấn luyện câu hỏi này và quay về trang chủ?."
  );
}
$(".btn-select-field").on("click", function () {
  var selectedValues = [];

  $('input[name="checkbox_field"]:checked').each(function () {
    selectedValues.push($(this).val());
  });
  if (selectedValues.length <= 0) {
    $(".modal-message").html(
      `<span class="fs-2 d-flex align-items-center justify-content-center text-danger">Vui lòng chọn lĩnh vực</span>`
    );
    return;
  } else {
    localStorage.setItem("SelectedFields", selectedValues);
    var fieldNames = "";
    selectedValues.forEach((x) => {
      var field = fields.filter((k) => x == k.qaFieldId)[0];
      var fieldColor = listStatusField.filter((k) => k.id == field.statusId)[0];
      fieldNames += `<span class="badge py-1 mx-1  ${
        fieldColor["color"] ?? "bg-success"
      }" title="${fieldColor["name"]}"> ${field.qaFieldName} </span>`;
    });
    $(".field-content").html(fieldNames);
  }
  getDataRequest.multipleFieldId = selectedValues.join(",");
  getDataRequest.questionId = null;
  $("#error-modal").modal("hide");
  getQuestionTraining();
});

function editQuestionField() {
  openRemoteModalNotRefresh(
    `qaquestion/editfield/${qaquestionid}`,
    "Cập nhật lĩnh vực của câu hỏi",
    "500px",
    "150px"
  );
}
function changeFieldInTraining(fieldId) {
  closeRemoteModalNotRefresh();
  if (
    getDataRequest.multipleFieldId.includes(fieldId) == false ||
    userFieldId.includes(fieldId) == false
  ) {
    var selectedValues = [];

    $('input[name="checkbox_field"]:checked').each(function () {
      selectedValues.push($(this).val());
    });
    getDataRequest.multipleFieldId =
      selectedValues.length > 0
        ? selectedValues.join(",")
        : userFieldId.join(",");
    getQuestionTraining();
  }
}
function changeFieldLabel(fieldname, qfid) {
  var statusId = fields.filter((x) => x.qaFieldId == qfid)[0]["statusId"];
  var fieldColor = listStatusField.filter((x) => x.id == statusId)[0];
  var field = `<span class="badge mx-1 ${
    fieldColor["color"] ?? "bg-success"
  }"> ${fieldname} </span>`;
  $(".question-field").html(field);
  $("#selectDocField").val(qfid);
  $(".select2-selection__rendered").html(fieldname);
  loadDocsByField(qfid);
  getRelevantQuestion(qfid, $(".question-content").html());
  onCancel_Click(false);
}
function ScrollToEnd(id, par) {
  if (!Scroller[par].DISABLE_SCROLL && !Scroller[par].SCROLLING) {
    Scroller[par].SCROLLING = true;
    setTimeout(function () {
      if (!Scroller[par].DISABLE_SCROLL && Scroller[par].SCROLLING) {
        $(id).animate(
          {
            scrollTop: $(document).height(),
          },
          250,
          "swing"
        );
      }
      Scroller[par].SCROLLING = false;
    }, 250);
  }
  return true;
}

var userFieldId = [];

var getNeedToTrainRequest = JSON.parse(JSON.stringify(getDataRequest));
var getWaitingToTrainRequest = JSON.parse(JSON.stringify(getDataRequest));
var isCanLoadNeedToTrainRequest = true;
var isLoadingNeedToTrainRequest = false;
var isCanLoadWaitingToTrainRequest = true;
var isLoadingWaitingToTrainRequest = false;
var Scroller = {
  internet: {
    DISABLE_SCROLL: false,
    SCROLLING: false,
  },
  now: {
    DISABLE_SCROLL: false,
    SCROLLING: false,
  },
};

var SelectedFields =
  localStorage.getItem("SelectedFields") == null
    ? []
    : localStorage.getItem("SelectedFields").split(",");
$(document).ready(function () {
  $(".btn-split-qs").prop("disabled", true);
  if (userField != null) {
    if (SelectedFields == null || SelectedFields.length == 0) {
      userField.forEach((x) => {
        SelectedFields.push(x.qaFieldId);
      });
    }
    $.each(userField, function (index, item) {
      var selected = SelectedFields.some((x) => x == item.qaFieldId);
      var checkbox = $(
        '<input type="checkbox" name="checkbox_field" ' +
          (selected == true ? "checked" : "") +
          ' value="' +
          item.qaFieldId +
          '"/>'
      );
      var fieldStatus =
        listStatusField.filter((x) => x.id == item.statusId).length > 0
          ? listStatusField.filter((x) => x.id == item.statusId)[0]
          : listStatusField[0];
      var label = $(
        `<span class="mx-2 fs-1" style="color:${fieldStatus["text_color"]}"> ${
          item.qaFieldDisplayName ?? item.qaFieldName
        }</span>`
      );
      var div = $('<div class="d-flex align-items-center">');
      checkbox.appendTo(div);
      label.appendTo(div);
      div.appendTo(".modal-field");
      userFieldId.push(item.qaFieldId);
      getDataRequest.multipleFieldId =
        SelectedFields.length == 0
          ? userFieldId.length > 0
            ? userFieldId.join(",")
            : ""
          : SelectedFields.join(",");
      getNeedToTrainRequest.multipleFieldId =
        SelectedFields.length == 0
          ? userFieldId.length > 0
            ? userFieldId.join(",")
            : ""
          : SelectedFields.join(",");
      getWaitingToTrainRequest.multipleFieldId =
        SelectedFields.length == 0
          ? userFieldId.length > 0
            ? userFieldId.join(",")
            : ""
          : SelectedFields.join(",");
    });
  }

  getQuestionTraining();
  $.each(fields, function (index, x) {
    var html =
      '<option value="' +
      x.qaFieldId +
      '" class="" >' +
      x.qaFieldName +
      "</option>";
    // var html = generateHTML(x);
    $("#selectDocField").append(html);
  });
  $(".tableNeedToTrain").on("scroll", function () {
    var scrollHeight = $(this).prop("scrollHeight");
    var scrollTop = $(this).prop("scrollTop");
    var client = $(this).prop("clientHeight");
    if (
      client + scrollTop >= scrollHeight - 10 &&
      isCanLoadNeedToTrainRequest &&
      !isLoadingNeedToTrainRequest
    ) {
      getNeedToTrainRequest.pageIndex += 1;
      getNeedToTrainRequest.isLoadNeedToTrain = true;
      loadMoreTrainingData(
        "NeedToTrain",
        getNeedToTrainRequest,
        "questionNeedToTrainLoading",
        "#questionNeedToTrain",
        ".question-need-train",
        renderQuestionNeedToTrain
      );
    } else {
      $("#questionNeedToTrainLoading").remove();
    }
  });
  $(".tableWaitingToTrain").on("scroll", function () {
    var scrollHeight = $(this).prop("scrollHeight");
    var scrollTop = $(this).prop("scrollTop");
    var client = $(this).prop("clientHeight");
    if (
      client + scrollTop >= scrollHeight - 10 &&
      isCanLoadWaitingToTrainRequest &&
      !isLoadingWaitingToTrainRequest
    ) {
      getWaitingToTrainRequest.pageIndex += 1;
      getWaitingToTrainRequest.isLoadWaitingToTrain = true;
      loadMoreTrainingData(
        "WaitingToTrain",
        getWaitingToTrainRequest,
        "questionWaitingToTrainLoading",
        "#questionWaitingToTrain",
        ".question-waiting-train",
        renderQuestionNeedToTrain
      );
    } else {
      $("#questionWaitingToTrainLoading").remove();
    }
  });

  $("#tab-question-internet, #tab-question-now").on("scroll", function () {
    var scrollHeight = $(this).prop("scrollHeight");
    var scrollTop = $(this).prop("scrollTop");
    var client = $(this).prop("clientHeight");
    if (client + scrollTop < scrollHeight) {
      Scroller[
        $(this).hasClass("now") == true ? "now" : "internet"
      ].DISABLE_SCROLL = true;
    } else {
      Scroller[
        $(this).hasClass("now") == true ? "now" : "internet"
      ].DISABLE_SCROLL = false;
    }
  });
});

function changeQuestionType(type, html) {
  if (type != null) {
    getDataRequest.questionType = type;
    $(".btn-question-source").html(html);
    getQuestionTraining();
  }
}
function changeContentLabel(qid, content) {
  $(".QuestionContent_" + qid).each(function () {
    $(this).text(content);
    if (qid == qaquestionid) {
      searchContent = content;
      getRelevantQuestion(originFieldId, content);
    }
  });
  onCancel_Click(false);
}
function loadMoreTrainingData(
  requestType,
  requestData,
  loadingClass,
  containerSelector,
  countSelector,
  renderFunction
) {
  let isLoadingFlag =
    requestType === "NeedToTrain"
      ? "isLoadingNeedToTrainRequest"
      : "isLoadingWaitingToTrainRequest";
  let isCanLoadFlag =
    requestType === "NeedToTrain"
      ? "isCanLoadNeedToTrainRequest"
      : "isCanLoadWaitingToTrainRequest";

  window[isLoadingFlag] = true;
  $(`.${loadingClass}`).remove();
  $(containerSelector).append(`
        <tr class="${loadingClass}">
            <td colspan="10">
                <div class="row">
                    <div class="col d-flex justify-content-center">
                        <div class="spinner-grow spinner-grow-sm" role="status">
                            <span class="visually-hidden">Đang tải dữ liệu...</span>
                        </div>
                    </div>
                </div>
            </td>
        </tr>`);

  $.ajax({
    url: "training/GetTrainingData",
    data: requestData,
    type: "POST",
    success: function (result) {
      $(`.${loadingClass}`).remove();

      if (result.message != null) {
        openAlertModal("Đã xảy ra lỗi", result.message);
      } else {
        $(countSelector).html(
          result.numberOfQuestionsNeedToTrain || result.waitingCount
        );
        let questions = result.questionNeedToTrain || result.waitingTrain;
        if (questions && questions.length > 0) {
          questions.forEach((x, i) => {
            let htmlContent = renderFunction(
              x,
              (requestData.pageIndex - 1) * requestData.pageSize + i,
              requestType === "NeedToTrain" ? true : false
            );
            $(containerSelector).append(htmlContent);
          });

          $(".questionNeedToTrainClick").on("click", function () {
            var id = $(this).data("id");
            getDataRequest.questionId = id;
            getQuestionTraining();
          });
        } else {
          window[isCanLoadFlag] = false;
        }
        window[isLoadingFlag] = false;
      }
    },
    error: function (err) {
      console.log(err);
    },
  });
}

async function getAiAnswer(model = "ChatGPT") {
  var postion =
    model.toLowerCase() == "chatgpt"
      ? ".question-answer-internet"
      : ".question-answer-now";
  var disableButton = model.toLowerCase() == "chatgpt" ? ".chatgpt" : ".ailuat";
  var loading = `<div class=" ${
    model.toLowerCase() == "chatgpt" ? "loading-gpt" : "loading-ailuat"
  }"><div class="col d-flex justify-content-center"><div class="spinner-grow spinner-grow-sm" role="status"><span class="visually-hidden">Đang tải dữ liệu...</span></div></div></div>`;

  var selectedQuestionId = 0;
  $(".form-check-input").each(function () {
    if ($(this).is(":checked")) {
      selectedQuestionId = $(this).data("id");
    }
  });
  var getNewAnswerRequest = {
    QAQuestionId: qaquestionid,
    PrimaryQuestionId: selectedQuestionId,
    AnswerType: model,
    UserId: user,
    SecretKey: jwt,
  };
  if (qaquestionid <= 0) {
    openAlertModal("Lỗi", "Không tìm thấy câu hỏi.");
    return;
  }
  $(postion).html(loading);
  try {
    $(disableButton).prop("disabled", true);
    const myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");
    myHeaders.append("Authentication", jwt);
    const raw = JSON.stringify(getNewAnswerRequest);
    const requestOptions = {
      method: "POST",
      headers: myHeaders,
      body: raw,
      redirect: "follow",
    };
    const response = await fetch(
      genTextDomain + "/sec-chat-training",
      requestOptions
    );
    const reader = response.body.getReader();
    const decoder = new TextDecoder("utf-8");
    var i = 0,
      content = "";
    while (true) {
      const { done, value } = await reader.read();
      const chunk = decoder.decode(value);
      if (typeof value === "undefined" && typeof done === "undefined") {
        break;
      }
      if (done) {
        $(disableButton).prop("disabled", false);
        var contentKatex = renderKatex(content);
        var contentRender = strDoc(contentKatex);
        $(postion).html("<md-block>" + contentRender + `</md-block>`);
        makeToast("success", "Đã hoàn thành tạo câu trả lời!");
        return true;
      } else if (chunk) {
        if (i == 0) {
          $(postion).html("");
        }
        if (chunk.startsWith('{"mid":') == true) {
        } else if (chunk.includes('{"mid":') == true) {
          var chunk1 = chunk.split("{")[0];
          var chunk2 = "{" + chunk.split("{")[1];
          content += chunk1;
          if (chunk2.startsWith('{"mid":') == true) {
          }
        } else {
          content += chunk;
        }
        var contentKatex = renderKatex(content);
        var contentRender = strDoc(contentKatex);
        contentRender += `<span class='loader1'></span>`;
        $(postion).html("<md-block>" + contentRender + `</md-block>`);
        ScrollToEnd(
          `#tab-question-${
            model.toLowerCase() == "chatgpt" ? "internet" : "now"
          }`,
          model.toLowerCase() == "chatgpt" ? "internet" : "now"
        );
        i++;
      }
    }
  } catch (err) {
    $(disableButton).prop("disabled", false);
    $(postion).html(
      "<span class='text-danger'>Xảy ra lỗi trong quá trình phản hồi, bạn vui lòng thử lại!</span>"
    );
    console.log(err);
  }
}
function addDocChunkByUser() {
  openRemoteModalNotRefresh(
    "training/addManualDocChunk?questionId=" +
      qaquestionid +
      "&fieldId=" +
      originFieldId,
    "Thêm đoạn văn bản tự biên soạn",
    "50vw",
    "700px"
  );
}
function editContentDocChunk(id) {
  openRemoteModalNotRefresh(
    "training/editDocChunkContent/" + id,
    "Thêm đoạn văn bản tự biên soạn",
    "50vw",
    "700px"
  );
}
function DocChunkSuccess(strMess) {
  getDocChunk(qaquestionid);
  makeToast("success", strMess);
  closeRemoteModalNotRefresh();
}
function SplitQuestion() {
  openRemoteModalNotRefresh(
    "training/SplitQuestion?questionId=" + qaquestionid,
    "Tách câu hỏi",
    "60vw",
    "500px"
  );
}
function SplitSuccess() {
  getDataRequest.questionId = null;
  getQuestionTraining();
  makeToast("success", "Đã thêm câu hỏi chi tiết");
  closeRemoteModalNotRefresh();
}
function showTraceLog() {
  openRemoteModalNotRefresh(
    `/ChatMessageTraceLog?keyword=&questionId=${qaquestionid}`,
    "Logs",
    "80vw",
    "90%"
  );
}
function ChangeField() {
  var listData = [];
  $(".selection_question_box").each(function () {
    if ($(this).is(":checked")) {
      listData.push($(this).val());
    }
  });
  if (listData.length == 0) {
    var message = "Vui lòng chọn câu hỏi trước";
    makeToast("error", message);
    openAlertModal("Thông báo", message);
    return;
  }

  openRemoteModalNotRefresh(
    `/qaquestion/EditFieldForMultipleQuestion?ids=${listData.join(",")}`,
    "Đổi lĩnh vực cho nhiều câu hỏi",
    "25vw",
    "25%"
  );
}
