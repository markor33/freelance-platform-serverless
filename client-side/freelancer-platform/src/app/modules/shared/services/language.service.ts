import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Language } from '../models/language.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {

  httpOptions = {
    headers: { 'Content-Type': 'application/json' }
  };

  constructor(private httpClient: HttpClient) { }

  get(): Observable<Language[]> {
    return this.httpClient.get<Language[]>(`api/freelancer/language`);
  }

}
