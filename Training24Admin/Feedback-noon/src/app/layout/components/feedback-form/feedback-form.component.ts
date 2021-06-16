// https://medium.com/@bryanjenningz/how-to-record-and-play-audio-in-javascript-faa1b2b3e49b
import { Component, OnInit, ViewChild, Input, SimpleChanges } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbAccordion, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Categories } from './../../../shared';
import { UploadEvent, UploadFile } from 'ngx-file-drop';
import { UtilService, GradeService, EmailPattern, FeedbackService } from './../../../shared';
import { NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import * as RecordRTC from 'recordrtc';
import { DomSanitizer } from '@angular/platform-browser';
import * as _ from 'lodash';
import * as moment from 'moment';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';


@Component({
  selector: 'noon-public-feedback-form',
  templateUrl: './feedback-form.component.html',
  styleUrls: ['./feedback-form.component.scss']
})
export class PublicFeedbackFormComponent implements OnInit {
  @ViewChild('contactDetailForm') contactDetailForm: NgForm;
  @ViewChild('feedbackAccordin') feedbackAccordin: NgbAccordion;
  @ViewChild('feedbackTimeModelContent') feedbackTimeModelContent;

  @Input('data') data;

  isFormSubmitted: boolean;
  contactDetail: any = {};
  feedBackDetailModel: any = {};
  public categotyLists: Array<any> = [];
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
  public timeModelflag: boolean = true;
  public allSubscribers: Array<any> = [];
  public recordStartTime: any;
  //Lets initiate Record OBJ
  private record;
  //Will use this flag for detect recording
  private recording = false;
  //Url of Blob
  private url;
  private error;
  private recordUrlIndex: any = {
    index: -1,
    subIndex: -1
  };
  private isPreparingUrl: boolean;
  private player: any;
  private audioSeek: any;
  isPlaying: boolean;

  constructor(
    public gradeService: GradeService,
    public feedbackService: FeedbackService,
    public utilService: UtilService,
    public modalService: NgbModal,
    public httpClient: HttpClient,
    private domSanitizer: DomSanitizer
  ) { }

  ngOnInit() {
    this.addfeedBackTime();
    this.categotyLists = Categories;
    this.preInitData();
    // this.insertData('lessonsList', 70);
    // this.insertData('chaptersList', 20);   
  }




  ngOnChanges(changes: SimpleChanges) {
    let tempfeedbackdata = [];
    if (this.data.id) {
      this.feedBackDetailModel.categoryId = this.data.category.id;
      this.feedBackDetailModel.gradeId = this.data.grade.id;
      this.gradeChange(this.data.grade.id);
      this.feedBackDetailModel.courseId = this.data.course.id;
      this.courseChange(this.data.course.id);
      
      if (this.data.lesson != null) {
        this.feedBackDetailModel.lessonId = this.data.lesson.id;
      } else {
        this.feedBackDetailModel.chapterId = this.data.chapter.id;
      }
      if(this.data.contact) {
        this.contactDetail = this.data.contact;
      }
    } else {
      this.feedBackDetailModel = {};
    }
    if (this.data.feedbacktime) {
      for (let index = 0; index < this.data.feedbacktime.length; index++) {
        let obj = {
          'timeLabel': this.data.feedbacktime[index].time,
          'description': this.data.feedbacktime[index].description,
          'imageFiles': this.data.feedbacktime[index].feedbackTimeFiles
        }
        if (typeof this.data.feedbacktime[index].time == 'string' && (this.data.feedbacktime[index].time.split(":")).length == 2) {
          let timeSplitArr: any[] = this.data.feedbacktime[index].time.split(":");
          obj['time'] = { hour: timeSplitArr[0], minute: timeSplitArr[1], second: timeSplitArr[2] };
        }
        tempfeedbackdata.push(obj);
        /*  for (let j = 0; j < this.data.feedbacktime[index].feedbackTimeFiles.length; j++) {
           this.data.feedbacktime[index].feedbackTimeFiles[j]['name'] = this.data.feedbacktime[index].feedbackTimeFiles[j].filename;
         } */
      }
      this.data.feedbacktime = [];
      this.feedBackTimes = tempfeedbackdata;
    }

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
    // element.style.width = "100%";
    setTimeout(() => {
      try {
        if (localStorage.getItem('contact_info') && localStorage.getItem('contact_info') != 'null') {
          this.contactDetail = JSON.parse(localStorage.getItem('contact_info'));
          this.onAddContactDetailForm();
        }
      } catch (error) {
        this.contactDetail = {};
      }
    }, 100);
  }

  preInitData() {
    this.getGrades();
  }

  getGrades() {
    this.isCallingApi = true;
    this.gradeService.getGradeList({}).then((res: any) => {
      this.isCallingApi = false;
      this.gradeList = res;
    }).catch(err => {
      this.isCallingApi = false;
    });
  }

  cetegoryChange() {
    this.timeModelflag = true;
    if ([1, 4, 5, 6].indexOf(this.feedBackDetailModel.categoryId) > -1) {
      this.feedBackTimes = [];
      this.addfeedBackTime();
    }

    if (['2', '5'].indexOf('' + this.feedBackDetailModel.categoryId) > -1) {
      if (this.feedBackDetailModel.courseId) {
        this.courseChange(this.feedBackDetailModel.courseId, this.feedBackDetailModel.categoryId)
      }
    }

  }

  gradeChange(gradeId) {
    this.isCallingApi = true;
    this.allSubscribers.push(this.gradeService.getGradeCourse({}, gradeId).subscribe(res => {
      this.isCallingApi = false;
      this.gradeCourseList = res.data;
        // this.feedBackDetailModel.courseId = undefined;
    }, err => {
      this.isCallingApi = false;
      this.utilService.showErrorCall(err);
    }));
  }

  courseChange(courseId, categoryId?: any) {
    this.isCallingApi = true;
    if (this.feedBackDetailModel.categoryId != '6') {
      let model: any = {};
      if (['2', '5'].indexOf('' + this.feedBackDetailModel.categoryId) > -1) {
        model.type = this.feedBackDetailModel.categoryId;
        if (this.feedBackDetailModel.categoryId == 5) {
          model.type = 1;
        }
      }
      this.allSubscribers.push(this.gradeService.getLessions(model, courseId).subscribe(res => {
        this.isCallingApi = false;
        this.lessonsList = res.data;
        // this.feedBackDetailModel.lessonId = undefined;
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
    // console.log(this.feedBackDetailModel);
    this.isFormSubmitted = true;
    // localStorage.setItem('contact_info', JSON.stringify(this.contactDetail));
    // this.feedbackAccordin.toggle('contactDetail');
    //  this.feedbackAccordin.toggle('feedBackDetail');
    // this.addfeedBackTime();
    //  this.submitFeedback();
  }

  openfeedBackInputModal(index?: any) {
    if (this.feedBackDetailModel.categoryId == 5 || this.feedBackDetailModel.categoryId == 6) {
      this.timeModelflag = false;
    }
    this.tempFeedback = null;
    if (this.feedBackTimes[index]) {
      this.tempFeedback = this.feedBackTimes[index];
      this.tempFeedback['index'] = index;
      this.time = this.feedBackTimes[index].time;
      // this.time = { hour: 0, minute: 0, second: 0 };

    } else {
      this.time = { hour: 0, minute: 0, second: 0 };
    }
    this.feedbackModalRef = this.modalService.open(this.feedbackTimeModelContent, {
      backdrop: 'static',
      centered: true
    });
    console.log(this.feedBackDetailModel.categoryId);

  }
  dismiss() {
    this.feedbackModalRef.dismiss();
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
      timeLabel: '00:00:00',
      time: { hour: 0, minute: 0, second: 0 },
      description: '',
      audioFiles: [{}],
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

  onSelectFiles($event, feedBackActiveIndex) {

    if ($event.target.files && $event.target.files.length > 0) {
      for (let index = 0; index < $event.target.files.length; index++) {
        this.feedBackTimes[feedBackActiveIndex].imageFiles.push($event.target.files[index]);
      }
    }
  }

  selectFile(imageFilesInput) {
    let element: HTMLElement = document.getElementById(imageFilesInput) as HTMLElement;
    element.click();
  }

  public dropped(event: UploadEvent, index: number) {


    for (const droppedFile of event.files) {
      // Is it a file?
      if (droppedFile.fileEntry.isFile) {
        const fileEntry: any = droppedFile.fileEntry;
        fileEntry.file((file: File) => {
          // if (['jpg', 'jpeg', 'png', 'bmp', 'gif'].indexOf(file.name.split('.').pop().toLowerCase()) > -1)
          //   this.feedBackTimes[this.feedBackActiveIndex].imageFiles.push(file);
          this.feedBackTimes[index].imageFiles.push(file);
        });
      }
    }
  }

  public submitFeedback() {
    this.onAddContactDetailForm();
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
        "contactid": '' + res.data.id,
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
    console.log(this.uploadFeedbackIndex);
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
    // console.log(index);

    return new Promise((resolve, reject) => {
      this.formData = null;
      this.formData = new FormData();
      console.log(this.feedBackTimes);
      this.formData.append("Description", this.feedBackTimes[index].description);
      this.formData.append("Time", this.feedBackTimes[index].timeLabel);
      this.formData.append("FeedbackId", this.feedBackId);
      for (let fIndex = 0; fIndex < this.feedBackTimes[index].imageFiles.length; fIndex++) {
        this.formData.append("files", this.feedBackTimes[index].imageFiles[fIndex]);
      }

      let duration: any = [];

      for (let fIndex = 0; fIndex < this.feedBackTimes[index].audioFiles.length; fIndex++) {
        if (this.feedBackTimes[index].audioFiles[fIndex].audiourl) {
          this.formData.append("files", this.feedBackTimes[index].audioFiles[fIndex].audiourl, this.feedBackTimes[index].audioFiles[fIndex].audiourl.name);
          duration.push({
            name: this.feedBackTimes[index].audioFiles[fIndex].audiourl.name,
            duration: this.feedBackTimes[index].audioFiles[fIndex].timeduration
          });
        }
      }

      this.formData.append('duration', JSON.stringify(duration));
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

  /**
     * Start recording.
     */
  initiateRecording(index, subindex, flag) {
    if (this.isPreparingUrl) {
      this.utilService.showErrorWarning("Please wait", "Audio recording in progress..");
      return;
    }
    if (flag) {
      this.stopRecording(index, subindex);
      return;
    }
    let findIndex = _.findIndex(this.feedBackTimes[index].audioFiles[subindex], { recording: true });
    if (findIndex > -1) {
      if (confirm("Sure to delete")) {
        this.stopRecording(index, findIndex);
        this.startRecording(index, subindex);
      } else
        return
      // todo code for deletion
    } else {
      this.startRecording(index, subindex);
    }

  }

  startRecording(index, subindex) {
    this.feedBackTimes[index].audioFiles[subindex]['recording'] = true;
    this.recording = true;
    let mediaConstraints = {
      video: false,
      audio: true
    };
    navigator.mediaDevices
      .getUserMedia(mediaConstraints)
      .then(this.successCallback.bind(this), (obj) => {
        this.feedBackTimes[index].audioFiles[subindex]['recording'] = false;
        console.log(obj);
        alert("Recording device not found");
      });
  }
  /**
   * Will be called automatically.
   */
  successCallback(stream) {
    var options = {
      mimeType: "audio/wav",
      numberOfAudioChannels: 1
    };
    //Start Actuall Recording
    var StereoAudioRecorder = RecordRTC.StereoAudioRecorder;
    this.record = new StereoAudioRecorder(stream, options);
    this.record.record();
    this.recordStartTime = new Date().getTime();
    if (this.player && !this.player.paused) {
      let fIndex = -1;
      let pIndex = -1;
      if (this.isPlaying) {
        for (let i = 0; i < this.feedBackTimes.length; i++) {
          fIndex = _.findIndex(this.feedBackTimes[i].audioFiles, { isPlaying: true });
          if (fIndex > -1) {
            pIndex = i;
            break;
          }
        }
      }
      if (fIndex > -1 && pIndex > -1) {
        this.stopPlaying(fIndex, pIndex);
      }
    }
  }

  stopPlaying(index, subindex) {
    this.isPlaying = false;
    this.feedBackTimes[index].audioFiles[subindex].isPlaying = false;
    this.player.pause();
    this.player.currentTime = 0;
    this.player = null;
  }
  /**
   * Stop recording.
   */
  stopRecording(index, subindex) {
    this.feedBackTimes[index].audioFiles[subindex]['recording'] = false;
    this.recording = false;
    this.isPreparingUrl = true;
    this.recordUrlIndex = {
      index: index,
      subindex: subindex
    };
    this.record.stop(this.processRecording.bind(this));
  }
  /**
   * processRecording Do what ever you want with blob
   * @param  {any} blob Blog
   */
  processRecording(blob) {
    // this.url = URL.createObjectURL(blob);
    var audioElement = document.createElement('audio');
    audioElement.preload = 'metadata';
    audioElement.onloadedmetadata = () => {
      window.URL.revokeObjectURL(audioElement.src);
      if (audioElement.duration < 1) {
        alert("Invalid Audio! audio is less than 1 second");
        return;
      } else {
        this.feedBackTimes[this.recordUrlIndex.index].audioFiles[this.recordUrlIndex.subindex].duration = this.getTime(_.clone(audioElement.duration));
        this.feedBackTimes[this.recordUrlIndex.index].audioFiles[this.recordUrlIndex.subindex].timeduration = _.clone(audioElement.duration);
        // this.feedBackTimes[this.recordUrlIndex.index].audioFiles[this.recordUrlIndex.subindex].audiourl = _.clone(audioElement.src);
        this.feedBackTimes[this.recordUrlIndex.index].audioFiles[this.recordUrlIndex.subindex].audiourl = this.blobToFile(blob, new Date().getTime() + ".mp3");
        // this.feedBackTimes[this.recordUrlIndex.index].audioFiles[this.recordUrlIndex.subindex].audiourl = blob;
        this.feedBackTimes[this.recordUrlIndex.index].audioFiles[this.recordUrlIndex.subindex].audioSeek = 0;
        this.feedBackTimes[this.recordUrlIndex.index].audioFiles.push({});
        this.isPreparingUrl = false;
        audioElement.remove();
      }
    }
    audioElement.src = window.URL.createObjectURL(blob);
    // var myFile = this.blobToFile(blob, "my-image.mp3");
    // console.log(myFile);
    // audioElement.src = this.blobToFile(blob, "my-image.mp3");

  }

  public blobToFile = (theBlob: Blob, fileName: string): File => {
    var b: any = theBlob;
    //A Blob() is almost a File() - it's just missing the two properties below which we will add
    b.lastModifiedDate = new Date();
    b.name = fileName;

    //Cast to a File() type
    return <File>theBlob;
  }
  /**
   * Process Error.
   */
  // errorCallback(error) {
  //   if(error){

  //   }

  //   this.error = 'Can not play audio in your browser';
  //   this.isPreparingUrl = false;
  // }

  audioSeekChange(flag, index, subindex) {
    this.player = document.getElementById('player_' + index + '_' + subindex);
    if (flag == 'start') {
      if (this.isPlaying)
        this.player.pause();
    } else {
      // console.log((this.feedBackTimes[index].audioFiles[subindex].audioSeek * 100) / this.player.duration);
      this.player.currentTime = (this.feedBackTimes[index].audioFiles[subindex].audioSeek * this.player.duration) / 100;
      this.player.play();
    }
  }

  togglePlay(index, subindex) {
    let fIndex = -1;
    let pIndex = -1;
    if (this.isPlaying) {
      for (let i = 0; i < this.feedBackTimes.length; i++) {
        fIndex = _.findIndex(this.feedBackTimes[i].audioFiles, { isPlaying: true });
        if (fIndex > -1) {
          pIndex = i;
          break;
        }
      }
    }
    if (fIndex > -1 && pIndex > -1) {
      // this.isPlaying = false;
      // this.feedBackTimes[pIndex].audioFiles[fIndex].isPlaying = false;
      // this.player.pause();
      // this.player.currentTime = 0;
      this.stopPlaying(pIndex, fIndex);
      setTimeout(() => {
        this.player = null;
        this.togglePlay(index, subindex);
      }, 100);
    } else {
      this.player = document.getElementById('player_' + index + '_' + subindex);
      if (!this.player.src) {
        let reader = new FileReader();
        reader.onload = (e: any) => {
          this.player.src = e.target.result;
        };
        this.player.ontimeupdate = () => {
          this.initProgressBar(index, subindex);
        };
        reader.readAsDataURL(this.feedBackTimes[index].audioFiles[subindex].audiourl);
      }
      if (this.player.paused === false) {
        this.player.pause();
        this.isPlaying = false;
        this.feedBackTimes[index].audioFiles[subindex].isPlaying = false;
      } else {
        this.player.load();
        this.player.play();
        this.isPlaying = true;
        this.feedBackTimes[index].audioFiles[subindex].isPlaying = true;
      }
    }
  }


  initProgressBar(index, subindex) {
    let player: any = document.getElementById('player_' + index + '_' + subindex);
    if (player) {
      let length = player.duration;
      let current_time: any = player.currentTime;
      this.feedBackTimes[index].audioFiles[subindex].audioSeek = (current_time / length) * 100;

      if (parseInt(length) == current_time) {
        this.isPlaying = false;
        this.feedBackTimes[index].audioFiles[subindex].isPlaying = false;
        this.player.stop();
      }
    }
    // console.log(this.audioSeek);

    // let totalLength: any = this.getTime(length)
    // console.log("end-time", totalLength);
    // var currentTime = this.getTime(current_time);
    // console.log("start-time", currentTime);
  }

  downloadFile(file) {
    if(file.url.includes("audio")){    
      window.open(file.url);
  // this.feedbackService.getaudio(file.url).subscribe((res) => {
  // var record = new Blob([res.body],{type: res.headers.get('audio/wav')});
  // const a = document.createElement('a');
  // a.href = URL.createObjectURL(record);
  // a.download = file.name;
  // document.body.appendChild(a);
  // a.click();
// })
    }else{
      this.feedbackService.getImage(file.url).subscribe(
        (res) => {
          var img = new Blob([res.body], { type: res.headers.get('Content-Type') }); 
          const a = document.createElement('a');
          a.href = URL.createObjectURL(img);
          a.download = file.name;
          document.body.appendChild(a);
          a.click();
        });
      }
  }

  getTime(length: any) {
    return moment.utc(parseInt(length) * 1000).format('HH:mm:ss');
  }

  ngOnDestroy() {
    this.allSubscribers.map(value => value.unsubscribe());
  }
}