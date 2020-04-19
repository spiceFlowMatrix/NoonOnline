package com.ibl.apps.AssignmentManagement;

import com.google.gson.JsonObject;
import com.ibl.apps.Model.AssignmentDetailObject;
import com.ibl.apps.Model.AssignmentObject;
import com.ibl.apps.Model.AssignmentSubmission;
import com.ibl.apps.Model.RestResponse;
import com.ibl.apps.Model.UploadTopicFile;
import com.ibl.apps.Model.assignment.AssignmentData;
import com.ibl.apps.Model.assignment.CommentResponse;
import com.ibl.apps.Model.assignment.FileUploadResponse;
import com.ibl.apps.Model.assignment.StudentDetailData;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.noon.NoonApplication;

import java.util.ArrayList;

import io.reactivex.Single;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import retrofit2.Call;

public class AssignmentRepository implements AssignmentApiService {
    private AssignmentApiService assignmentApiService;

    public AssignmentRepository() {
        assignmentApiService = ApiClient.getClient(NoonApplication.getContext()).create(AssignmentApiService.class);
    }

    @Override
    public Single<AssignmentObject> fetchAssignments(String Id) {
        return assignmentApiService.fetchAssignments(Id);
    }

    @Override
    public Single<RestResponse<ArrayList<StudentDetailData>>> assignmentStudentDetail(String assignmentid) {
        return assignmentApiService.assignmentStudentDetail(assignmentid);
    }

    @Override
    public Single<RestResponse<ArrayList<StudentDetailData>>> assignmentStudentDetailChapter(String assignmentid) {
        return assignmentApiService.assignmentStudentDetailChapter(assignmentid);
    }

    @Override
    public Single<AssignmentSubmission> assignmentAddComment(JsonObject body) {
        return assignmentApiService.assignmentAddComment(body);
    }

    @Override
    public Single<AssignmentSubmission> assignmentAddCommentChapter(JsonObject body) {
        return assignmentApiService.assignmentAddCommentChapter(body);
    }

    @Override
    public Single<RestResponse<CommentResponse>> getAssignmentComments(String pagenumber, String perpagerecord, String topicid, String studentid) {
        return assignmentApiService.getAssignmentComments(pagenumber, perpagerecord, topicid, studentid);
    }

    @Override
    public Single<RestResponse<CommentResponse>> getAssignmentCommentsChapter(String pagenumber, String perpagerecord, String topicid, String studentid) {
        return assignmentApiService.getAssignmentCommentsChapter(pagenumber, perpagerecord, topicid, studentid);
    }

    @Override
    public Single<AssignmentDetailObject> fetchAssignmentsDetails(String Id) {
        return assignmentApiService.fetchAssignmentsDetails(Id);
    }

    @Override
    public Call<UploadTopicFile> UploadTopicFile(MultipartBody.Part image, RequestBody fileTypeId, RequestBody duration, RequestBody filesize) {
        return assignmentApiService.UploadTopicFile(image, fileTypeId, duration, filesize);
    }

    @Override
    public Call<RestResponse<FileUploadResponse>> uploadAssignmentFile(MultipartBody.Part file, RequestBody fileTypeId, RequestBody duration, RequestBody filesize, RequestBody totalpages) {
        return assignmentApiService.uploadAssignmentFile(file, fileTypeId, duration, filesize, totalpages);
    }

    @Override
    public Single<RestResponse<AssignmentData>> submitAssignmentLession(JsonObject body) {
        return assignmentApiService.submitAssignmentLession(body);
    }

    @Override
    public Single<RestResponse<AssignmentData>> submitAssignment(JsonObject body) {
        return assignmentApiService.submitAssignment(body);
    }

    @Override
    public Single<RestResponse<AssignmentData>> addTeaherTopicSubmission(JsonObject body) {
        return assignmentApiService.addTeaherTopicSubmission(body);
    }

    @Override
    public Single<RestResponse<AssignmentData>> addTeaherTopicSubmissionChapter(JsonObject body) {
        return assignmentApiService.addTeaherTopicSubmissionChapter(body);
    }
}
