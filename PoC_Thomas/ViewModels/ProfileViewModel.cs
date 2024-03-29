﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using PoC_Thomas.Commons;
using PoC_Thomas.Helpers.Interface;
using PoC_Thomas.Models.Entities;
using Prism.Commands;
using Prism.Navigation;
using SQLite;
using Xamarin.Forms;

namespace PoC_Thomas.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        public DelegateCommand<CharacterEntity> DeleteCommand { get; set; }
        public DelegateCommand<CharacterEntity> ViewCommand { get; set; }

        public ProfileViewModel(INavigationService navigationService, ISqliteNetHelper sqliteNetHelper) : base(navigationService, sqliteNetHelper)
        {
            DeleteCommand = new DelegateCommand<CharacterEntity>(DeleteCharacter);
            ViewCommand = new DelegateCommand<CharacterEntity>(ViewCharacter);
        }


        #region OnNavigatedToAsync
        protected override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);

            Characters = new ObservableCollection<CharacterEntity>(await SqliteNetHelper.GetCharacters(App.UserId));
            UserEntity = await SqliteNetHelper.GetUser(App.UserId);
        }
        #endregion


        // This function delete a character from the database
        public async void DeleteCharacter(CharacterEntity character)
        {
            await SqliteNetHelper.DeleteCharacter(character.Id, App.UserId);
            Characters.Remove(character);
        }

        public async void ViewCharacter(CharacterEntity character)
        {
            var parameter = new NavigationParameters { { "character", character } };
            await NavigationService.NavigateAsync(Constants.CharacterPage, parameter);
        }
        

        private UserEntity _userEntity;
        public UserEntity UserEntity
        {
            get { return _userEntity; }
            set { SetProperty(ref _userEntity, value); }
        }


        private ObservableCollection<CharacterEntity> _characters;
        public ObservableCollection<CharacterEntity> Characters
        {
            get { return _characters; }
            set { SetProperty(ref _characters, value); }
        }
    }
}
