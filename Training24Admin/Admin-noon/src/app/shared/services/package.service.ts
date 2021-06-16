import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class PackageService {
constructor(private commonapiservice: CommonAPIService) {

}

getPackages(filter) {
    return this.commonapiservice.get('v1/Package/GetPackages', filter);
}

getPackageById(id) {
    return this.commonapiservice.get('v1/Package/GetPackage'+ (id ? '/' + id : ''))
}

managePackage(data) {
    if (data.id)
            return this.commonapiservice.put('v1/Package/UpdatePackage',data);
        else
            return this.commonapiservice.post('v1/Package/AddPackage', data);
}

getPackageCourse(filterObj, id?: string) {
    return this.commonapiservice.get('v1/PackageCourse/GetPackageCourses' + (id ? '/' + id : ''), filterObj);
}


addPackageCourse(dataObj) {
    return this.commonapiservice.post('v1/PackageCourse/AddPackageCourse', dataObj);
}

deleteCourseFromPackage(id) {
    return this.commonapiservice.delete('v1/PackageCourse/DeletePackageCourse/' + id);
}

deletePackage(id) {
    return this.commonapiservice.delete('v1/Package/DeletePackage/' + id);
}
}