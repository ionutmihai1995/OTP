import { Observable } from 'rxjs';
import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpErrorResponse, HttpClient, HttpResponse, HttpEvent } from "@angular/common/http";
import { map, catchError } from 'rxjs/operators';
import { throwError as observableThrowError } from 'rxjs';

@Injectable()
export class OTPInterceptor implements HttpInterceptor {

    constructor(private http: HttpClient) { }


    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const clonedRequest = req.clone({
            responseType: 'text'
        });

        return next.handle(clonedRequest).pipe(
            map((event: HttpEvent<any>) => {
                if (event instanceof HttpResponse) {
                    return event.clone({
                        body: this.isJson(event.body) ? JSON.parse(event.body) : event.body,
                    });
                }
            }),
            catchError((error: HttpErrorResponse) => {
                
                const parsedError = Object.assign({}, error, { error: this.isJson(error.error) ? JSON.parse(error.error) : error.error });
                return observableThrowError(new HttpErrorResponse(parsedError));
            })
        );
    }

    isJson(str) {
        try {
            JSON.parse(str);
        } catch (e) {
            return false;
        }
        return true;
    }
}