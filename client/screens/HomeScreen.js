import { useNavigation } from '@react-navigation/native'
import React, { useEffect, useLayoutEffect, useState } from 'react'
import { SafeAreaView, ScrollView, StatusBar, Text, TextInput, View } from 'react-native'
import * as Icon from "react-native-feather"
import { getFeaturedResturants } from '../api'
import Categories from '../components/categories'
import FeatureRow from '../components/featuredRow'

export default function HomeScreen() {

    const [featuredCategories, setFeaturedCategories] = useState([])
    const navigation = useNavigation();
    useLayoutEffect(() => {
      navigation.setOptions({headerShown: false})
    }, [])
    useEffect(()=>{
        getFeaturedResturants().then(data=>{
            setFeaturedCategories(data);
        })
    },[])

  return (
    <SafeAreaView className="bg-white pt-3 flex-1">
    <StatusBar
        barStyle="dark-content" 
    />
    {/* search bar */}
        <View className="flex-row items-center space-x-2 px-4 pb-2 ">
            <View className="flex-row flex-1 items-center p-3 rounded-full border border-gray-300">
                <Icon.Search height="25" width="25" stroke="gray" />
                <TextInput placeholder='Resturants' className="ml-2 flex-1" keyboardType='default' />
                <View className="flex-row items-center space-x-1 border-0 border-l-2 pl-2 border-l-gray-300">
                    <Icon.MapPin height="20" width="20" stroke="gray" />
                    <Text className="text-gray-600">Nha Trang</Text>
                </View>
            </View>
            <View style={{backgroundColor: 'gray'}} className="p-3 rounded-full">
                <Icon.User height={20} width={20} strokeWidth="2.5" stroke="white" />
            </View>
        </View>

         

    {/* main */}
    <ScrollView
        showsVerticalScrollIndicator={false}
        contentContainerStyle={{
            paddingBottom: 50
        }}
    >
       
        {/* categories */}
        <Categories />

        {/* featured */}
        <View className="mt-5">
        {
            featuredCategories?.map(category=>{
                return (
                        <FeatureRow 
                            key={category._id}
                            id={category._id}
                            title={category.name}
                            resturants={category?.resturants}
                            description={category.description}
                            featuredCategory={category._type}
                        />
                )
            })
        }
        </View>
        

        
       
    </ScrollView>
      
    </SafeAreaView>
  )
}