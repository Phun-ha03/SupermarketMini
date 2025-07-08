
function changeAssignee(issueId, userId, item) {
	var avatarstr = "";
	$.ajax({
		url: '@this.Url.Action("AssignForUserAjax", "Issue")',
		data: { issueId: issueId, userId: userId },
		type: 'Post',
		success: function (data) {
			console.log(data);
			if (data.resultObj != "0") {
				const avatar = $(item).data('avatar');

				$('#assigneeAvatar').attr('src', avatar);
				$('#assigneeAvatar2').attr('src', avatar);
				avatarstr = avatar;
				//alert(@temp);
			}
		}
	});
}

function changeSprint(issueId, sprintId, item) {
	$.ajax({
		url: '@this.Url.Action("UpdateSprintAjax", "IssueSprints")',
		data: { issueSprintId: issueId, sprintId: sprintId },
		type: 'Post',
		success: function (data) {
			if (data.resultObj != "0") {
				console.log($(item).data('sprintname'));

				$('#spintName').text($(item).data('sprintname'));
			}
		}
	});
}

function changePriority(issueId, priorityId, item) {
	$.ajax({
		url: '@this.Url.Action("UpdatePriorityAjax", "Issue")',
		data: { issueId: issueId, priorityId: priorityId },
		type: 'Post',
		success: function (data) {
			console.log($(item).data('colorpriority'));
			if (data.resultObj != "0") {
				$('#priorityName').text($(item).data('priorityname'));
				$('#priorityIcon').css("color", $(item).data('colorpriority'));
				$('#priorityIcon').removeClass($('#priorityIcon').data('cls')).addClass($(item).data('ic'));
			}
		}
	});
}

function changeTitle(issueId, title) {
	$.ajax({
		url: '@this.Url.Action("UpdateIssueNameAjax", "Issue")',
		data: { issueId: issueId, title: title },
		type: 'Post',
		success: function (data) {
			if (data.resultObj != "0") {
				console.log('dfdfdf');
			}
		}
	});
}

function changeIssueDesc(issueId, issueDesc) {
	$.ajax({
		url: '@this.Url.Action("UpdateIssueDescAjax", "Issue")',
		data: { issueId: issueId, issueDesc: issueDesc },
		type: 'Post',
		success: function (data) {
			if (data.resultObj != "0") {
				console.log('dfdfdf');
			}
		}
	});
}
function addComment(userId, issueId) {
	var content = $('#content_comment').val();
	$.ajax({
		url: '@this.Url.Action("CreateAjax", "Comment")',
		data: { userId: userId, content: content, issueId: issueId },
		type: 'Post',
		success: function (data) {

			if (data.length > 0) {
				$('#comment_list').prepend(data);
				$('#content_comment').val('');
			}
		}
	});
}
function createActivity(actionTypeId,actionUserId,affectedObjectId, message) {
	$.ajax({
		url: '@this.Url.Action("CreateAjax", "Activities")',
		data: { actionTypeId: actionTypeId, actionUserId: actionUserId, affectedObjectId: affectedObjectId, message: message },
		type: 'Post',
		success: function (data) {
			alert('CreateAjax');
			if (data.resultObj != "0") {
				console.log('dfdfdf');
			}
		}
	});
}


function linkify(text) {
	var text2 = text.replace(/<(\/*)a[^>]*>/g, '<$1##>');
	//console.log(text2);
	//console.log(text);
	var exp = /(\b(https?|ftp|file):\/\/[-A-Z0-9+&@():'#\/%?=~_|!:,.;]*[-A-Z0-9+&@():'#\/%=~_|])/ig;
	//console.log(text.replace(exp, "<a href='$1' target='_blank'>$1</a>"));
	return text.replace(exp, "<a href='$1' target='_blank'>$1</a>");
}
