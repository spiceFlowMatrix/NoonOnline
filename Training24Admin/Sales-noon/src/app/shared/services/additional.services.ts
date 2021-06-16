import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class AdditionalService {
constructor(private commonapiservice: CommonAPIService) {

}

getServices(filterObj) {
    return this.commonapiservice.get('v1/AddtionalServices/GetServices',filterObj);
}

// getServicesById(id) {
//     return this.commonapiservice.get('v1/AddtionalServices/GetService'+ (id ? '/' + id : ''))
// }

// manageService(data: any){    
//     if (data.id)
//         return this.commonapiservice.put('v1/AddtionalServices/UpdateService', data );
//     else
//         return this.commonapiservice.post('v1/AddtionalServices/AddService', data);
// }

// deleteService(id) {
//     return this.commonapiservice.delete('v1/AddtionalServices/DeleteService/' + id);
  
// }
}