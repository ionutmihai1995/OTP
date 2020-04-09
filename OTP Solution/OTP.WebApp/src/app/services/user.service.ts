import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { ValidityResult } from '../models/validity-result';

@Injectable()
export class UserService {

    constructor(private http: HttpClient) { }

    checkValidity = (userId: string): Observable<ValidityResult> => {
        return this.http.post<ValidityResult>(`${environment.baseUrl}/user/checkValidity`, { userId: userId });
    }

    login = (userId: string, generatedOTP: string, secretKeyGuid: string): Observable<any> => {
        return this.http.post(`${environment.baseUrl}/user/login`, { userId: userId, generatedOTP : generatedOTP, secretKeyGuid : secretKeyGuid });
    }
}