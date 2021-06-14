import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AddChaptersRoutingModule } from './add-chapters-routing.module';
import { AddChaptersComponent } from './add-chapters.component';
import { PageHeaderModule, SharedModule, CourseService, ChapterService, QuizService } from '../../../shared';
import {MatInputModule} from '@angular/material/input';
import { MatCommonModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import {MatButtonModule} from '@angular/material/button';
import {MatCardModule} from '@angular/material/card';


@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        MatCommonModule,
        MatFormFieldModule,
        MatButtonModule,
        MatCardModule,
        MatInputModule,
        SharedModule.forRoot(),
        AddChaptersRoutingModule,
    ],
    declarations: [AddChaptersComponent],
    providers: [ChapterService, CourseService, QuizService]
})
export class AddChaptersModule { }
