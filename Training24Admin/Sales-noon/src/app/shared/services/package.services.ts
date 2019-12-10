import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class PackageService {
constructor(private commonapiservice: CommonAPIService) {

}

getPackages(filter) {
    return this.commonapiservice.get('v1/Package/GetPackages', filter);
}
}
