import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { UtilService, ChapterService, CourseService, DataService } from '../../../shared';
import * as _ from 'lodash';

@Component({
    selector: 'app-add-chapters',
    templateUrl: './add-chapters.component.html',
    styleUrls: ['./add-chapters.component.scss']
})
export class AddChaptersComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('manageChapterForm') manageChapterForm: NgForm;
    public chapterModel: any = {};
    isEditView: boolean = false;
    isCallingApi: boolean = false;
    // courseList: any = [];
    quizList: any = [];

    constructor(
        private activatedRoute: ActivatedRoute,
        public utilService: UtilService,
        public chapterService: ChapterService,
        public courseService: CourseService,
        public dataService: DataService,
        public router: Router
    ) {
        this.allSubscribers.push(this.activatedRoute.params.subscribe((params: Params) => {
            if (params['id']) {
                this.isEditView = true;
                this.chapterModel.id = params['id'];
                this.getChapterById(this.chapterModel.id);
            } else {
                this.chapterModel.code = this.utilService.getRandomCode('CH');
                this.isEditView = false;
            }
        }));

    }

    ngOnInit() {
        setTimeout(() => {
            window.scrollTo( 0, 0 ); // how far to scroll on each step
        }, 500);
        this.allSubscribers.push(this.activatedRoute.queryParams.subscribe((params: Params) => {
            if (params['courseid']) {
                this.chapterModel.courseid = params['courseid'];
                if (params['index'])
                    this.chapterModel.name = 'Chapter ' + params['index'];
                this.courseService.getCourseList().then((res: any) => {
                    let fIndex = _.findIndex(res, (o: any) => {
                        return o.id == this.chapterModel.courseid
                    })

                    if (fIndex > -1) {
                        this.chapterModel.coursename = res[fIndex].name
                    }
                }).catch(err => err);
            }
        }));
    }

    getChapterById(id) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.chapterService.getChapterById(id).subscribe(res => {
            this.isCallingApi = false;
            Object.keys(res.data).forEach(key => {
                this.chapterModel[key] = res.data[key];
            });
            // this.chapterModel = _.clone(res.data);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    manageChapter() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.chapterService.manageChapter(_.clone(this.chapterModel)).subscribe((res: any) => {
            this.isCallingApi = false;
            delete this.chapterModel.itemorder;
            if (!this.isEditView)
                this.router.navigateByUrl('/courses/course-preview/' + this.chapterModel.courseid + '/edit-chapter/' + res.data.id + window.location.search);
            else
                this.router.navigateByUrl('/courses/course-preview/' + this.chapterModel.courseid + '/edit-chapter/' + this.chapterModel.id + window.location.search);
            this.dataService.refreshCourseData({
                operation: 'add',
                type: 'chapter',
                obj: res.data
            });

            // if (this.backViewLink)
            //     this.router.navigateByUrl('/courses/course-preview/' + this.backViewLink);
            // else
            //     this.router.navigate(['chapters']);
        }, err => {
            debugger
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
