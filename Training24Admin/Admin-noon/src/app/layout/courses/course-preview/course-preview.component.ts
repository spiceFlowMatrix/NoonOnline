import { ViewChild, Component, OnInit, Output, EventEmitter } from '@angular/core';
import { UtilService, DELETE_TITLE, CourseService, ChapterService, LessonService, DataService, QuizService, AssignmentService } from '../../../shared';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Observable, of, Subject, merge, interval, timer } from 'rxjs';
import { debounce, debounceTime, distinctUntilChanged, filter, map } from 'rxjs/operators';
import { DragulaService } from 'ng2-dragula';
import * as _ from 'lodash';
@Component({
    selector: 'app-coursepreview',
    templateUrl: './course-preview.component.html',
    styleUrls: ['./course-preview.component.scss']
})
export class CoursePreviewComponent implements OnInit {
    @ViewChild('deleteConfirmationDialog') deleteConfirmationDialog: any;
    focus$ = new Subject<string>();
    chaptersSubscriber: any = null;
    allSubscribers: Array<any> = [];
    courseId: any = {};
    selectedCourse: any;
    coursePreviewObj: any = {};
    coursesList: any = [];
    expandedList: any = [];
    isEditView: boolean;
    isCallingApi: boolean;
    modalRef: any = null;

    constructor(
        private courseService: CourseService,
        public utilService: UtilService,
        private activatedRoute: ActivatedRoute,
        private dragulaService: DragulaService,
        private dataService: DataService,
        private chapterService: ChapterService,
        private lessonService: LessonService,
        private quizService: QuizService,
        public assignmentService: AssignmentService,
        public router: Router
    ) {
        this.isEditView = true;
        this.allSubscribers.push(this.activatedRoute.params.subscribe((params: Params) => {
            if (params['id']) {
                this.courseId = params['id'];
                setTimeout(() => {
                    this.loadCoursePreview(this.courseId);
                }, 1000);
            } else {
                this.utilService.showErrorWarning("You can't access page without course id.")
                window.history.back();
            }
        }));

        this.dataService.courseDataRefresh$.subscribe(res => {
            // console.log(res);
            setTimeout(() => {
                this.loadCoursePreview(this.courseId, true);
            }, 1000);
        });

        if (!this.dragulaService.find("CHAPTERS")) {
            let chapterGroup = this.dragulaService.createGroup("CHAPTERS", {
                direction: 'vertical',
                moves: (el, source, handle) => handle.className === "card-header"
            });

            chapterGroup.drake.on('dragend', (el) => {
                let chaptersModel = [];
                for (let index = 0; index < this.coursePreviewObj.chapters.length; index++) {
                    chaptersModel.push({
                        chapterid: this.coursePreviewObj.chapters[index].id,
                        chaptername: this.coursePreviewObj.chapters[index].name,
                        itemorder: index + 1
                    });
                }
                this.isCallingApi = true;
                this.allSubscribers.push(this.courseService.ChapterOrderChange(chaptersModel).subscribe(res => {
                    this.isCallingApi = false;
                }, err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                }));

            });
            let lessonGroup = this.dragulaService.createGroup("LESSONS", {
                direction: 'vertical',
                // moves: (el: any, source, handle) => {
                //     console.log(el.classList.contains('quiz-item'));
                //     // console.log(el.classList,el.classList.indexOf('quiz-item') < 0);

                //     return !el.classList.contains('quiz-item');
                // }
            });

            this.dragulaService.dropModel("LESSONS")
                .subscribe(({ name, el, target, source, sibling, sourceModel, targetModel, item }) => {
                    let model = {
                        previousdetail: null,
                        newdetail: null
                    }

                    model.newdetail = {};
                    model.newdetail['chapterid'] = target.getAttribute('data-chapter');
                    model.newdetail['lessondetaillist'] = [];
                    for (let index = 0; index < targetModel.length; index++) {
                        model.newdetail['lessondetaillist'].push({
                            id: targetModel[index].id,
                            itemorder: index + 1,
                            type: targetModel[index].type
                            // isQuiz: !targetModel[index].lessonfiles ? true : false
                        });
                    }
                    // console.log(targetModel);

                    if (target.getAttribute('data-chapter') !== source.getAttribute('data-chapter')) {
                        model.previousdetail = {};
                        model.previousdetail['chapterid'] = source.getAttribute('data-chapter');
                        model.previousdetail['lessondetaillist'] = [];
                        for (let index = 0; index < sourceModel.length; index++) {
                            model.previousdetail['lessondetaillist'].push({
                                id: sourceModel[index].id,
                                itemorder: index + 1,
                                isQuiz: !sourceModel[index].lessonfiles ? true : false
                            });
                        }
                    }

                    this.isCallingApi = true;

                    this.courseService.lessonOrderChange(model).subscribe(res => {
                        this.isCallingApi = false;
                    }, err => {
                        this.isCallingApi = false;
                        this.utilService.showErrorCall(err);
                    });
                });
        }
    }

    focusOnCourseselection() {
        setTimeout(() => {
            document.getElementById('courseSelectionInput').focus();
        }, 500);
    }

    ngOnInit() {
        this.getCourses();
        this.dataService.setSideMenu(true);
    }

    ngAfterViewInit() {
        this.toggleScroll(false);
    }

    courseChangeEvent(event) {
        this.loadCoursePreview(event);
    }

    toggleScroll(flag) {
        let element: HTMLBodyElement = document.getElementsByTagName("body")[0];
        if (flag) {
            element.classList.remove('hide-overflow');
        } else {
            element.classList.add('hide-overflow');
        }
    }

    formatter = (x: { name: string }) => x.name;
    rformatter = (result: any) => result.name.toUpperCase();

    search = (text$: Observable<string>) => {
        const debouncedText$ = text$.pipe(debounceTime(200), distinctUntilChanged());
        const inputFocus$ = this.focus$;

        return merge(debouncedText$, inputFocus$).pipe(
            map(term => (term === '' ? this.coursesList
                : this.coursesList.filter(v => v.name.toLowerCase().indexOf(term.toLowerCase()) > -1)))
        );
    }

    selectCourse($event) {
        // console.log($event);
        if (this.selectedCourse.id != $event.item.id) {
            this.selectedCourse = _.clone(_.find(this.coursesList, (obj) => {
                if ('' + obj.id == $event.item.id) {
                    return true;
                }
                return false;
            }));
            this.router.navigateByUrl('/courses/course-preview/' + this.selectedCourse.id);
        }
        // this.selectedCourse = $event.item.id;
    }

    // changeCourse() {
    //     console.log(this.courseId);
    //     this.router.navigateByUrl('/courses/course-preview/' + this.courseId);
    // }

    deleteConfirmationPopup(type, chapterIndex, lessonIndex?: number) {
        let tempObj = null;
        switch (type) {
            case 'lesson':
                tempObj = this.coursePreviewObj.chapters[chapterIndex].lessons[lessonIndex];
                break;
            case 'chapter':
                tempObj = this.coursePreviewObj.chapters[chapterIndex];
                tempObj.type = type;
                break;
        }
        tempObj.chapterIndex = chapterIndex;
        tempObj.lessonIndex = lessonIndex;
        this.deleteConfirmationDialog.open(1, DELETE_TITLE, 'Are you sure want to delete "' + tempObj.name + '" ?', tempObj);
    }

    deleteItem(object) {
        switch (object.type) {
            case 1:
                this.removeLesson(object);
                break;
            case 2:
                this.removeQuiz(object);
                break;
            case 3:
                this.removeAssignment(object);
                break;
            case 'chapter':
                this.removeChapter(object);
                break;
        }
    }

    removeLesson(data) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.lessonService.deleteLesson(data.id).subscribe(res => {
            this.isCallingApi = false;
            this.coursePreviewObj.chapters[data['chapterIndex']].lessons.splice(data['lessonIndex'], 1);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    removeAssignment(data) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.assignmentService.deleteAssignment(data.id).subscribe(res => {
            this.isCallingApi = false;
            this.coursePreviewObj.chapters[data['chapterIndex']].lessons.splice(data['lessonIndex'], 1);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    removeQuiz(data) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.quizService.deleteQuiz(data.id).subscribe(res => {
            this.isCallingApi = false;
            this.coursePreviewObj.chapters[data['chapterIndex']].lessons.splice(data['lessonIndex'], 1);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    removeChapter(data) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.chapterService.deleteChapter(data.id).subscribe(res => {
            this.isCallingApi = false;
            this.coursePreviewObj.chapters.splice(data['chapterIndex'], 1);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    openLesson(chapter, lesson) {
        // if (lesson && lesson.lessonfiles) {
        //     this.router.navigate(['/courses/course-preview/' + this.coursePreviewObj.id + '/edit-lesson/' + lesson.id], {
        //         queryParams: {
        //             chapterid: chapter.id,
        //             type: lesson.lessonfiles[0].files.filetypename,
        //             chaptername: chapter.name,
        //             courseid: this.coursePreviewObj.id
        //         }
        //     });
        // }
        if (lesson.type == 1) {
            this.router.navigate(['/courses/course-preview/' + this.coursePreviewObj.id + '/edit-lesson/' + lesson.id], {
                queryParams: {
                    chapterid: chapter.id,
                    // type: lesson.lessonfiles[0].files.filetypename,
                    chaptername: chapter.name,
                    courseid: this.coursePreviewObj.id
                }
            });
        }
        else if (lesson.type == 2) {
            this.router.navigate(['/courses/course-preview/' + this.coursePreviewObj.id + '/edit-quiz/' + lesson.id], {
                queryParams: {
                    chapterid: chapter.id,
                    type: 'quiz',
                    chaptername: chapter.name,
                    courseid: this.coursePreviewObj.id
                }
            });
        }
        else {
            this.router.navigate(['/courses/course-preview/' + this.coursePreviewObj.id + '/edit-assignment/' + lesson.id], {
                queryParams: {
                    chapterid: chapter.id,
                    // type: 'quiz',
                    chaptername: chapter.name,
                    courseid: this.coursePreviewObj.id
                }
            });
        }
    }

    getCourses() {
        this.isCallingApi = true;
        this.courseService.getCourseList().then(res => {
            this.coursesList = res;
            this.selectedCourse = _.find(this.coursesList, (obj) => {
                if ('' + obj.id == this.courseId) {
                    return true;
                }
                return false;
            });
        }).catch(err => err);
    }

    loadCoursePreview(courseId, refreshPanels?: boolean) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.courseService.coursePreview(courseId).subscribe(res => {
            this.isCallingApi = false;
            this.coursePreviewObj = res.data;
            // console.log(res.data);
            if (this.coursePreviewObj.chapters && this.coursePreviewObj.chapters.length > 0) {
                if (!refreshPanels) {
                    this.coursePreviewObj.chapters[0].isExpanded = true;
                    this.chapterExpanded(this.coursePreviewObj.chapters[0].id, true);
                }
                for (let index = 0; index < this.coursePreviewObj.chapters.length; index++) {
                    if (refreshPanels)
                        this.coursePreviewObj.chapters[index].isExpanded = (this.expandedList.indexOf(this.coursePreviewObj.chapters[index].id) > -1) ? TrackEvent : false;
                    this.coursePreviewObj.chapters[index].lessons = _.orderBy(this.coursePreviewObj.chapters[index].lessons, ['itemorder'], ['asc']);
                }
            }
        }, err => {
            this.coursePreviewObj = {};
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    chapterExpanded(id, openFlag) {
        if (openFlag) {
            this.expandedList.push(id);
        } else {
            if (this.expandedList.indexOf(id) > -1)
                this.expandedList.splice(this.expandedList.indexOf(id), 1);
        }
    }

    ngOnDestroy() {
        this.toggleScroll(true);
        this.dataService.setSideMenu(false);
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
