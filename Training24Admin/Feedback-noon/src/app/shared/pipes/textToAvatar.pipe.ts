import {
    Pipe,
    PipeTransform
} from '@angular/core';

@Pipe({
    name: 'textToAvatar'
})
export class TextToAvatarPipe implements PipeTransform {
   
    transform(value: string): any {
        return value ? value.substring(0, 1).toUpperCase() :'';
    }
}