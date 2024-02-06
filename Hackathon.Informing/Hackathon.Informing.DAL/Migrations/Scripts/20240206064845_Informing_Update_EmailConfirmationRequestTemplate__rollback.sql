update public."InformingTemplates"
set "Content" = '<!DOCTYPE html><html lang="en"><head><meta charset="UTF-8"><title>Подтверждение Email</title></head><body>' ||
                'Уважаемый [username]>, подтвердите свой Email перейдя по сссылке указанной ниже или указав код подтверждения ' ||
                'в настройках профиля. <br/><a href="[confirmationLink]">[confirmationLink]</a><br/>' ||
                'Код подтверждения: <h3>[confirmationCode]</h3></body></html>'
where "Id" = 'EmailConfirmationRequestTemplate';
