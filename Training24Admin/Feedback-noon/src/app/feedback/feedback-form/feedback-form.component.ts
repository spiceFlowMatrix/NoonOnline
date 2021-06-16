import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbAccordion, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Categories } from '../../shared';
import { UploadEvent, UploadFile } from 'ngx-file-drop';
import { UtilService, GradeService, EmailPattern, FeedbackService } from '../../shared';
import { NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-feedback-form',
  templateUrl: './feedback-form.component.html',
  styleUrls: ['./feedback-form.component.scss']
})
export class FeedbackFormComponent implements OnInit {
  @ViewChild('contactDetailForm') contactDetailForm: NgForm;
  @ViewChild('feedbackAccordin') feedbackAccordin: NgbAccordion;
  @ViewChild('feedbackTimeModelContent') feedbackTimeModelContent;
  isFormSubmitted: boolean;
  contactDetail: any = {};
  feedBackDetailModel: any = {};
  public categotyLists: Array<any> = [];
  public allSubscribers: Array<any> = [];
  public isCallingApi: boolean;
  public isFeedbackUploading: boolean;
  public gradeList: any = [];
  public gradeCourseList: any = [];
  public lessonsList: any = [];
  public chaptersList: any = [];
  public feedBackTimes: any = [];
  public feedBackActiveIndex: number;
  public emailPattern: string = EmailPattern;
  public feedBackId: any;
  public time: NgbTimeStruct = { hour: 0, minute: 0, second: 0 };
  public tempFeedback: any = null;
  public feedbackModalRef: any = null;
  public uploadFeedbackSubScriber: any = null;
  public uploadFeedbackIndex: number;
  public formData: any = null;
  public isValiationError: boolean;

  constructor(
    public gradeService: GradeService,
    public feedbackService: FeedbackService,
    public utilService: UtilService,
    public modalService: NgbModal
  ) { }

  ngOnInit() {
    this.categotyLists = Categories;
    this.preInitData();
    // this.insertData('lessonsList', 70);
    // this.insertData('chaptersList', 20);
  }
  insertData(key, length) {
    for (let index = 0; index < length; index++) {
      this[key].push({
        id: index + 1,
        name: index + 1
      });
    }
  }

  ngAfterViewInit() {
    this.utilService.allowOnlyNumber("phone_num_code");
    this.utilService.allowOnlyNumber("phone_num");
    let element: HTMLButtonElement = document.querySelector('#contactDetail-header h5 button');
    if (element)
      element.style.width = "100%";
    // setTimeout(() => {
    //   try {
    //     if (localStorage.getItem('contact_info') && localStorage.getItem('contact_info') != 'null') {
    //       this.contactDetail = JSON.parse(localStorage.getItem('contact_info'));
    //       this.onAddContactDetailForm();
    //     }
    //   } catch (error) {
    //     this.contactDetail = {};
    //   }
    // }, 100);
  }

  preInitData() {
    this.getGrades();
  }

  getGrades() {
    this.isCallingApi = true;
    this.allSubscribers.push(this.gradeService.getGrades({}).subscribe(res => {
      this.isCallingApi = false;
      this.gradeList = res.data;
    }, err => {
      this.isCallingApi = false;
      this.utilService.showErrorCall(err);
    }));
  }

  cetegoryChange() {
    if ([1, 4, 5, 6].indexOf(this.feedBackDetailModel.categoryId) > -1) {
      this.feedBackTimes = [];
      this.addfeedBackTime();
    }
  }

  gradeChange(gradeId) {
    this.isCallingApi = true;
    this.allSubscribers.push(this.gradeService.getGradeCourse({}, gradeId).subscribe(res => {
      this.isCallingApi = false;
      this.gradeCourseList = res.data;
    }, err => {
      this.isCallingApi = false;
      this.utilService.showErrorCall(err);
    }));
  }

  courseChange(courseId) {
    this.isCallingApi = true;
    console.log(this.feedBackDetailModel.categoryId);

    if (this.feedBackDetailModel.categoryId != '6') {
      this.allSubscribers.push(this.gradeService.getLessions({}, courseId).subscribe(res => {
        this.isCallingApi = false;
        this.lessonsList = res.data;
      }, err => {
        this.isCallingApi = false;
        this.utilService.showErrorCall(err);
      }));
    }

    if (this.feedBackDetailModel.categoryId == '6') {
      this.allSubscribers.push(this.gradeService.getChapters({}, courseId).subscribe(res => {
        this.isCallingApi = false;
        this.chaptersList = res.data;
      }, err => {
        this.isCallingApi = false;
        this.utilService.showErrorCall(err);
      }));
    }
  }

  public beforeChange($event: any) {
    if (($event.panelId === 'feedBackDetail' && $event.nextState === true) && !this.isFormSubmitted) {
      this.utilService.showErrorWarning("", "Please fill contact form first...");
      $event.preventDefault();
    }
  }

  onAddContactDetailForm() {
    this.isFormSubmitted = true;
    // localStorage.setItem('contact_info', JSON.stringify(this.contactDetail));
    this.feedbackAccordin.toggle('contactDetail');
    this.feedbackAccordin.toggle('feedBackDetail');
    this.addfeedBackTime();
  }

  openfeedBackInputModal(index?: any) {
    this.tempFeedback = null;
    if (this.feedBackTimes[index]) {
      this.tempFeedback = this.feedBackTimes[index];
      this.tempFeedback['index'] = index;
      this.time = this.feedBackTimes[index].time;
    } else {
      this.time = { hour: 0, minute: 0, second: 0 };
    }
    this.feedbackModalRef = this.modalService.open(this.feedbackTimeModelContent, {
      backdrop: 'static',
      centered: true
    });
  }

  manageFeedbackTime() {
    this.feedbackModalRef.dismiss();
    let timeLabel = this.toModel(this.time);
    if (!this.tempFeedback) {
      this.addfeedBackTime();
      this.feedBackTimes[this.feedBackActiveIndex].time = this.time;
      this.feedBackTimes[this.feedBackActiveIndex].timeLabel = timeLabel;
    } else {
      this.feedBackTimes[this.tempFeedback.index].time = this.time;
      this.feedBackTimes[this.tempFeedback.index].timeLabel = timeLabel;
    }
  }

  toModel(time): string {
    if (!time) {
      return null;
    }
    return `${this.pad(time.hour)}:${this.pad(time.minute)}:${this.pad(time.second)}`;
  }

  private pad(i: number): string {
    return i < 10 ? `0${i}` : `${i}`;
  }

  addfeedBackTime() {
    this.feedBackTimes.push({
      timeLabel: '--:--',
      time: { hour: 0, minute: 0, second: 0 },
      description: '',
      audioFiles: [],
      imageFiles: []
    });
    this.feedBackActiveIndex = this.feedBackTimes.length - 1;
  }

  removeFeedbackTime(index) {
    if (this.feedBackActiveIndex == this.feedBackTimes.length - 1) {
      this.feedBackActiveIndex--;
    }
    this.feedBackTimes.splice(index, 1);

  }

  onSelectFiles($event) {
    if ($event.target.files && $event.target.files.length > 0) {
      for (let index = 0; index < $event.target.files.length; index++) {
        this.feedBackTimes[this.feedBackActiveIndex].imageFiles.push($event.target.files[index]);
      }
    }
  }

  selectFile(imageFilesInput) {
    let element: HTMLElement = document.getElementById(imageFilesInput) as HTMLElement;
    element.click();
  }

  public dropped(event: UploadEvent) {
    for (const droppedFile of event.files) {
      // Is it a file?
      if (droppedFile.fileEntry.isFile) {
        const fileEntry: any = droppedFile.fileEntry;
        fileEntry.file((file: File) => {
          if (['jpg', 'jpeg', 'png', 'bmp', 'gif'].indexOf(file.name.split('.').pop().toLowerCase()) > -1)
            this.feedBackTimes[this.feedBackActiveIndex].imageFiles.push(file);
        });
      }
    }
  }

  public submitFeedback() {
    // let formData = new FormData();
    // Object.keys(this.contactDetail).forEach(key => {
    //   formData.append(key, this.contactDetail[key]);
    // });
    this.isValiationError = false;
    if (['2', '3', '5', '6'].indexOf(this.feedBackDetailModel.categoryId) > -1) {
      if (!this.feedBackDetailModel.gradeId || !this.feedBackDetailModel.courseId
        || (this.feedBackDetailModel.categoryId != '6' && !this.feedBackDetailModel.lessonId)
        || (this.feedBackDetailModel.categoryId == '6' && !this.feedBackDetailModel.chapterId)) {
        this.isValiationError = true;
        this.utilService.showErrorToast("Required!", "Fields selection is mendatory.");
        return;
      }
    }
    // if (['1', '4'].indexOf(this.feedBackDetailModel.categoryId) > -1) {
    for (let index = 0; index < this.feedBackTimes.length; index++) {
      if (!this.feedBackTimes[index].description) {
        this.feedBackActiveIndex = index;
        this.utilService.showErrorToast("Required!", "Description is mendatory.");
        return;
      }
    }
    if (this.isCallingApi) {
      this.utilService.showErrorInfo('Please wait..', "Uploading In progress...");
      return;
    }
    this.isCallingApi = true;
    this.utilService.showErrorInfo('In progress...', "Uploading Contact detail.");
    this.allSubscribers.push(this.feedbackService.addContactDetail(this.contactDetail).subscribe(res => {
      let model: any = {
        "contactid": res.data.id,
        "categoryId": this.feedBackDetailModel.categoryId,
        "gradeId": this.feedBackDetailModel.gradeId,
        "chapterId": this.feedBackDetailModel.chapterId,
        "courseId": this.feedBackDetailModel.courseId,
        "lessonId": this.feedBackDetailModel.lessonId,
        "Description": this.feedBackDetailModel.Description,
      }
      this.utilService.showErrorInfo('In progress...', "Uploading Feedback detail.");
      this.feedbackService.addFeedback(model).subscribe(res => {
        this.feedBackId = res.data.id;
        this.saveFeedbackTimes(0);
      }, err => {
        this.isCallingApi = false;
        this.utilService.showErrorCall(err);
      })
    }, err => {
      this.isCallingApi = false;
      this.utilService.showErrorCall(err);
    }));
  }

  public saveFeedbackTimes(index) {
    if (this.isFeedbackUploading && this.uploadFeedbackIndex == this.feedBackTimes.length) {
      this.isFeedbackUploading = false;
      this.resetDetail();
    }

    if (this.uploadFeedbackSubScriber)
      this.uploadFeedbackSubScriber.unsubscribe();

    this.uploadFeedbackIndex = index;
    this.uploadFeedBackTime(this.uploadFeedbackIndex).then(res => {
      index++;
      this.moveToNextUpload(index);
    }).catch(err => {
      console.log(index, err);
      index++;
      this.moveToNextUpload(index);
    });

  }

  moveToNextUpload(index) {
    if (index < this.feedBackTimes.length) {
      this.saveFeedbackTimes(index);
    } else {
      this.isFeedbackUploading = false;
      this.resetDetail();
    }
  }

  uploadFeedBackTime(index) {
    console.log(index);

    return new Promise((resolve, reject) => {
      this.formData = null;
      this.formData = new FormData();
      this.formData.append("Description", this.feedBackTimes[index].description);
      this.formData.append("Time", this.feedBackTimes[index].timeLabel);
      this.formData.append("FeedbackId", this.feedBackId);
      for (let fIndex = 0; fIndex < this.feedBackTimes[index].imageFiles.length; fIndex++) {
        this.formData.append("files", this.feedBackTimes[index].imageFiles[fIndex]);
      }
      this.uploadFeedbackSubScriber = this.feedbackService.uploadFeedBackTime(this.formData).subscribe(res => {
        resolve(res);
      }, err => {
        reject(err);
      });
    });
  }

  resetDetail() {
    this.isCallingApi = false;
    this.utilService.showErrorSuccess('Congratulations', "Your feedback uploaded successfully.");
    setTimeout(() => {
      window.location.reload();
    }, 1500);
  }

  ngOnDestroy() {
    this.allSubscribers.map(value => value.unsubscribe());
  }
}