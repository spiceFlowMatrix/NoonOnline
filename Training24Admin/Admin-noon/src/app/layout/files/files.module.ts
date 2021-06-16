import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FilesRoutingModule } from './files-routing.module';
import { FilesComponent } from './files.component';

@NgModule({
    imports: [CommonModule, FilesRoutingModule],
    declarations: [FilesComponent]
})
export class FilesModule { }
