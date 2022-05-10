export class User
{
    id: number | undefined;
    login: string | undefined;
    password: string | undefined;
    fullname: string | undefined;
    token: any | undefined;
    image: any | undefined;

    constructor(login?:string, token?:string) {
        this.id = 0;
        this.image = '';
        this.login = login;
        this.token = token;
    }
}
