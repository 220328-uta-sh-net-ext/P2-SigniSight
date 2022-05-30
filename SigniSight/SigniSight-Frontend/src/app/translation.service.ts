import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { Observable, throwError, catchError } from 'rxjs';
import { IResponse } from './IResponse';

@Injectable({
  providedIn: 'root'
})
export class TranslationService {

  text:string = "";
  response:IResponse[] = [];

  
  //?textToTranslate=hi&endLanguageCode=ru
  translation(textToTranslate:string, endLanguageCode:string):Observable<any>{
    return this.http.post("https://localhost:7073/Translate?textToTranslate=" + textToTranslate +
    "&endLanguageCode="+ endLanguageCode, JSON.stringify({textToTranslate, endLanguageCode}),
    // We need to add headers to specify content type
    //{headers: {'Content-Type':'text/plain'}}
    )
    .pipe(
      catchError((e) =>{
        return throwError(e)
      }
     
    ))
  }
  constructor(private http:HttpClient) { }
}