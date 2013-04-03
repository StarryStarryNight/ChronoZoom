﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Application.Driver;
using Application.Helper.Entities;
using Application.Helper.UserActions;
using OpenQA.Selenium;

namespace Application.Helper.Helpers
{
    public class ExhibitHelper : DependentActions
    {
        public void AddExhibit(Exhibit exhibit)
        {
            Logger.Log("<- " + exhibit);
            InitExhibitCreationMode();
            SetExhibitPoint();
            SetExhibitTitle(exhibit.Title);
            SaveAndClose();
            Logger.Log("->");
        }

        public void AddExhibitWithContentItem(Exhibit exhibit)
        {
            Logger.Log("<- " + exhibit);
            InitExhibitCreationMode();
            SetExhibitPoint();
            SetExhibitTitle(exhibit.Title);
            AddContentItems(exhibit.ContentItems);
            SaveAndClose();
            Logger.Log("->");
        }

        public Exhibit GetNewExhibit()
        {
            Logger.Log("<-");
            const string script =
                "vc.data('ui-virtualCanvas')._layersContent.children[0].children[vc.data('ui-virtualCanvas')._layersContent.children[0].children.length - 1]";
            Exhibit exhibit = new Exhibit();
            ContentItem contentItem = new ContentItem();
            exhibit.ContentItems = new Collection<Chronozoom.Entities.ContentItem>();
            exhibit.Title =
                GetJavaScriptExecutionResult(script + ".title");
            int contentItemsCount = int.Parse(GetJavaScriptExecutionResult(script + ".contentItems.length"));
            Logger.Log("- contentItemsCount: " + contentItemsCount);
            for (int i = 0; i < contentItemsCount; i++)
            {
                string item = string.Format("{0}.contentItems[{1}].", script, i);
                contentItem.Title = GetJavaScriptExecutionResult(item + "title");
                contentItem.Caption = GetJavaScriptExecutionResult(item + "description");
                contentItem.MediaSource = GetJavaScriptExecutionResult(item + "uri");
                contentItem.MediaType = GetJavaScriptExecutionResult(item + "mediaType");
                exhibit.ContentItems.Add(contentItem);
            }
            exhibit.ExhibitId = GetJavaScriptExecutionResult(script + ".id");
            Logger.Log("->" + exhibit);
            return exhibit;
        }

        private void SaveAndClose()
        {
            Logger.Log("<-");
            Click(By.XPath("//*[@class='ui-dialog-buttonset']/*[1]"));
            Logger.Log("->");
        }

        private void SetExhibitTitle(string timelineName)
        {
            Logger.Log("<- name: " + timelineName);
            TypeText(By.Id("exhibitTitleInput"), timelineName);
            Logger.Log("->");
        }

        private void InitExhibitCreationMode()
        {
            Logger.Log("<-");
            MoveToElementAndClick(By.XPath("//*[@id='footer-authoring']/a[3]"));
            Logger.Log("->");
        }

        private void SetExhibitPoint()
        {
            Logger.Log("<-");
            MoveToElementAndClick(By.ClassName("virtualCanvasLayerCanvas"));
            Logger.Log("->");
        }

        private void AddContentItems(Collection<Chronozoom.Entities.ContentItem> contentItems)
        {
            Logger.Log("<- name: " + contentItems);
            for (int i = 0; i <= contentItems.Count - 1; i++)
            {
                ClickAddContentItem();
                SetTitle(contentItems[i].Title, i + 1);
                SetCaption(contentItems[i].Caption, i + 1);
                SetMediaSourse(contentItems[i].MediaSource, i + 1);
                SelectMediaType(contentItems[i].MediaType, i + 1);
            }
            Logger.Log("->");
        }

        private void SelectMediaType(string mediaType, int index)
        {
            Select(By.XPath(string.Format("(//*[@class='cz-auth-ci-media-type'])[{0}]", index)), mediaType);
        }

        private void SetMediaSourse(string mediaSourse, int index)
        {
            TypeText(By.XPath(string.Format("(//*[@class='cz-auth-ci-media-source'])[{0}]", index)), mediaSourse);
        }

        private void SetCaption(string description, int index)
        {
            TypeText(By.XPath(string.Format("(//*[@class='cz-auth-ci-description'])[{0}]", index)), description);
        }

        private void SetTitle(string title, int index)
        {
            TypeText(By.XPath(string.Format("(//*[@class='cz-auth-ci-title'])[{0}]", index)), title);
        }

        private void ClickAddContentItem()
        {
            Logger.Log("<-");
            Click(By.XPath("//*[@class='ui-dialog-buttonset']/*[2]"));
            Logger.Log("->");
        }
    }
}