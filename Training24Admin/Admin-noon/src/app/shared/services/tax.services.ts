import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class TaxService {
    constructor(private commonapiservice: CommonAPIService) {

    }
    manageTax(data) {
        return this.commonapiservice.put('v1/SalesTax/UpdateTax', data);
    }
    getTax() {
        return this.commonapiservice.get('v1/SalesTax/GetTax')
    }
}