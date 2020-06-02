
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Inject } from '@angular/core';


@Injectable({
  providedIn: 'root',
})
export class NodeService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  /**
   * This method will perform a GET operation
   * @param url The url to GET
   */
  fetchURL(url: string) {

    let promise = new Promise((resolve, reject) => {
      this.http.get<any>(this.baseUrl + url)
        .toPromise()
        .then(res => {
          resolve(res);
        }, msg => {
          reject(msg);
        })
    });

    return promise;
  }

  /**
   * This method will perform a PUT operation
   * @param url The url for the PUT operation
   * @param objectToPut The body of the PUT operation
   */
  putObject(url: string, objectToPut: object) {

    const headers = new HttpHeaders().set('content-type', 'application/json');

    let promise = new Promise((resolve, reject) => {
      this.http.put<any>(this.baseUrl + url, JSON.stringify(objectToPut), { headers })
        .toPromise()
        .then(res => {
          resolve(res);
        }, msg => {
          reject(msg);
        })
    });

    return promise;
  }

  /**
   * This method will perform a POST operation
   * @param url The url for the POST operation
   * @param objectToPost The body of the POST operation
   */
  postObject(url: string, objectToPost: object) {

    const headers = new HttpHeaders().set('content-type', 'application/json');

    let promise = new Promise((resolve, reject) => {
      this.http.post<any>(this.baseUrl + url, objectToPost, { headers })
        .toPromise()
        .then(res => {
          resolve(res);
        }, msg => {
          reject(msg);
        })
    });

    return promise;
  }

}
