import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TextToAvatarPipe } from './textToAvatar.pipe';
@NgModule({
    imports: [
        CommonModule
    ],
    declarations: [TextToAvatarPipe],
    exports: [TextToAvatarPipe],
    providers: []
})
export class SharedPipesModule { }
